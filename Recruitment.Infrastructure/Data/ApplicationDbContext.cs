using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces.Common;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Entities.Aduit;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Entities.Recruitment_Proccess;
using Recruitment.Domain.Entities.RecruitmentProccess;
using Recruitment.Domain.Entities.Reports;
using Recruitment.Domain.Entities.UserManagement;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text.Json;

namespace Recruitment.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public int CurrentUserId => GetCurrentUserId() ?? 0;

        public bool IsAdmin => _httpContextAccessor
        .HttpContext?
        .User?
        .IsInRole("Admin") ?? false;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // DbSets
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<Location> Locations { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<ProjectVacancy> ProjectVacancies { get; set; } = null!;
        public DbSet<Vacancy> Vacancies { get; set; } = null!;
        public DbSet<Title> Titles { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<DepartmentTitle> DepartmentTitles { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;
        public DbSet<Currency> Currencies { get; set; } = null!;

        // User Management
        public DbSet<Permission> Permissions { get; set; } = null!;
        public DbSet<RolePermission> RolePermissions { get; set; } = null!;
        public DbSet<Applicant> Applicants { get; set; } = null!;

        // Recruitment Process
        public DbSet<ApplicantApplication> Applications { get; set; }
        public DbSet<Interview> Interviews { get; set; }
        public DbSet<RejectionReason> RejectionReasons { get; set; } = null!;
        public DbSet<Interviewer> Interviewers { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportParameter> ReportParameters { get; set; }
        public DbSet<UserProject> UserProjects { get; set; }

        private string GetCurrentUsername()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
        }
        private int? GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor
                .HttpContext?
                .User?
                .FindFirst(ClaimTypes.NameIdentifier)?
                .Value;

            return userIdClaim != null ? int.Parse(userIdClaim) : null;
        }

        private void ApplyProjectFilters(ModelBuilder modelBuilder)
        {
            // Projects
            modelBuilder.Entity<Project>()
                .HasQueryFilter(p =>
                    IsAdmin ||
                    UserProjects.Any(up =>
                        up.UserId == CurrentUserId &&
                        up.ProjectId == p.Id));

            // ProjectVacancies
            modelBuilder.Entity<ProjectVacancy>()
                .HasQueryFilter(pv =>
                    IsAdmin ||
                    UserProjects.Any(up =>
                        up.UserId == CurrentUserId &&
                        up.ProjectId == pv.ProjectId));

            // Vacancies
            modelBuilder.Entity<Vacancy>()
                .HasQueryFilter(v =>
                    IsAdmin ||
                    v.ProjectVacancies!.Any(pv =>
                        UserProjects.Any(up =>
                            up.UserId == CurrentUserId &&
                            up.ProjectId == pv.ProjectId)));

            // Applications
            modelBuilder.Entity<ApplicantApplication>()
                .HasQueryFilter(a =>
                    IsAdmin ||
                    a.Vacancy.ProjectVacancies!.Any(pv =>
                        UserProjects.Any(up =>
                            up.UserId == CurrentUserId &&
                            up.ProjectId == pv.ProjectId)));

            // Interviews
            modelBuilder.Entity<Interview>()
                .HasQueryFilter(i =>
                    IsAdmin ||
                    i.Application.Vacancy.ProjectVacancies!.Any(pv =>
                        UserProjects.Any(up =>
                            up.UserId == CurrentUserId &&
                            up.ProjectId == pv.ProjectId)));

            // Locations
            modelBuilder.Entity<Location>()
                .HasQueryFilter(l =>
                    IsAdmin ||
                    l.Projects!.Any(p =>
                        UserProjects.Any(up =>
                            up.UserId == CurrentUserId &&
                            up.ProjectId == p.Id)));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var prop = Expression.Property(parameter, "IsDeleted");
                    var condition = Expression.Equal(prop, Expression.Constant(false));
                    var lambda = Expression.Lambda(condition, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProject>()
                .HasKey(up => new { up.UserId, up.ProjectId });

            modelBuilder.Entity<UserProject>()
                .HasIndex(up => up.UserId);

            ApplyProjectFilters(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            var auditEntries = new List<AuditLog>();

            foreach (var entry in ChangeTracker.Entries()
                .Where(e =>
                    e.Entity is BaseEntity &&
                    (e.State == EntityState.Added ||
                     e.State == EntityState.Modified ||
                     e.State == EntityState.Deleted)))
            {
                var entityName = entry.Entity.GetType().Name;
                var baseEntity = (BaseEntity)entry.Entity;
                var currentUser = GetCurrentUsername();

                // Handle Added
                if (entry.State == EntityState.Added)
                {
                    baseEntity.CreatedOn = DateTime.UtcNow;
                    baseEntity.CreatedBy ??= currentUser;
                }

                // Handle Modified
                else if (entry.State == EntityState.Modified)
                {
                    baseEntity.ModifiedOn = DateTime.UtcNow;
                    baseEntity.ModifiedBy ??= currentUser;
                }

                // Handle Deleted (soft delete with related data check)
                else if (entry.State == EntityState.Deleted &&
                         entry.Entity is ISoftDeletable softDeletable)
                {
                    // Check related collections for any not-deleted entities
                    var navigationProperties = entry.Navigations
                             .Where(n => n.Metadata.IsCollection)
                             .ToList();


                    foreach (var nav in navigationProperties)
                    {
                        if (!nav.IsLoaded)
                            await nav.LoadAsync(cancellationToken);

                        var relatedEntities = ((IEnumerable<object>)nav.CurrentValue!)
                            .OfType<ISoftDeletable>()
                            .Where(r => !r.IsDeleted)
                            .ToList();

                        if (relatedEntities.Any())
                        {
                            throw new InvalidOperationException(
                                $"Cannot delete {entityName} because it has related {nav.Metadata.Name}.");
                        }
                    }

                    // Apply soft delete
                    softDeletable.IsDeleted = true;
                    baseEntity.ModifiedOn = DateTime.UtcNow;
                    baseEntity.ModifiedBy ??= currentUser;

                    entry.State = EntityState.Modified;
                }

                // Prepare Audit Log
                var audit = new AuditLog
                {
                    TableName = entityName,
                    ActionType =
                        entry.State == EntityState.Modified &&
                        entry.Entity is ISoftDeletable sd &&
                        sd.IsDeleted
                            ? "Deleted"
                            : entry.State.ToString(),
                    ChangedBy = currentUser,
                    ChangedOn = DateTime.UtcNow
                };

                var keyValues = new Dictionary<string, object?>();
                var oldValues = new Dictionary<string, object?>();
                var newValues = new Dictionary<string, object?>();

                foreach (var prop in entry.Properties)
                {
                    var propName = prop.Metadata.Name;

                    if (prop.Metadata.IsPrimaryKey())
                        keyValues[propName] = prop.CurrentValue;

                    if (entry.State == EntityState.Added)
                    {
                        newValues[propName] = prop.CurrentValue;
                    }
                    else if (entry.State == EntityState.Modified && prop.IsModified)
                    {
                        oldValues[propName] = prop.OriginalValue;
                        newValues[propName] = prop.CurrentValue;
                    }
                }

                audit.KeyValues = JsonSerializer.Serialize(keyValues);
                audit.OldValues = oldValues.Any() ? JsonSerializer.Serialize(oldValues) : null;
                audit.NewValues = newValues.Any() ? JsonSerializer.Serialize(newValues) : null;

                auditEntries.Add(audit);
            }

            // Save main changes
            var result = await base.SaveChangesAsync(cancellationToken);

            // Save audit logs
            if (auditEntries.Any())
            {
                await AuditLogs.AddRangeAsync(auditEntries, cancellationToken);
                await base.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}
