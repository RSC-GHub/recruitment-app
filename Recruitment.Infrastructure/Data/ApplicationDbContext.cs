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
using System.Text.Json;

namespace Recruitment.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

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

        private string GetCurrentUsername()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
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
