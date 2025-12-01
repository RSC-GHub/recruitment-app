using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Recruitment.Domain.Entities;
using Recruitment.Domain.Entities.Aduit;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Entities.Recruitment_Proccess;
using Recruitment.Domain.Entities.RecruitmentProccess;
using Recruitment.Domain.Entities.UserManagement;
using System.Linq.Expressions;

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

        private string GetCurrentUsername()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = Expression.Parameter(entityType.ClrType, "e");
                    var prop = Expression.Property(parameter, nameof(BaseEntity.IsDeleted));
                    var condition = Expression.Equal(prop, Expression.Constant(false));
                    var lambda = Expression.Lambda(condition, parameter);
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditEntries = new List<AuditLog>();

            foreach (var entry in ChangeTracker.Entries().Where(e =>
                e.Entity is BaseEntity &&
                (e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted)))
            {
                var entityName = entry.Entity.GetType().Name;
                var baseEntity = (BaseEntity)entry.Entity;

                switch (entry.State)
                {
                    case EntityState.Added:
                        baseEntity.CreatedOn = DateTime.UtcNow;
                        baseEntity.CreatedBy ??= GetCurrentUsername();
                        break;

                    case EntityState.Modified:
                        baseEntity.ModifiedOn = DateTime.UtcNow;
                        baseEntity.ModifiedBy ??= GetCurrentUsername();
                        break;

                    case EntityState.Deleted:
                        baseEntity.ModifiedOn = DateTime.UtcNow;
                        baseEntity.ModifiedBy ??= GetCurrentUsername();
                        baseEntity.IsDeleted = true;
                        entry.State = EntityState.Modified;
                        break;
                }

                // Audit log
                var audit = new AuditLog
                {
                    TableName = entityName,
                    ActionType = entry.State.ToString(),
                    ChangedBy = GetCurrentUsername(),
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

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            newValues[propName] = prop.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            oldValues[propName] = prop.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (prop.IsModified)
                            {
                                oldValues[propName] = prop.OriginalValue;
                                newValues[propName] = prop.CurrentValue;
                            }
                            break;
                    }
                }

                audit.KeyValues = System.Text.Json.JsonSerializer.Serialize(keyValues);
                audit.OldValues = oldValues.Any() ? System.Text.Json.JsonSerializer.Serialize(oldValues) : null;
                audit.NewValues = newValues.Any() ? System.Text.Json.JsonSerializer.Serialize(newValues) : null;

                auditEntries.Add(audit);
            }

            // Save entity changes first
            var result = await base.SaveChangesAsync(cancellationToken);

            if (auditEntries.Any())
            {
                await AuditLogs.AddRangeAsync(auditEntries, cancellationToken);
                await base.SaveChangesAsync(cancellationToken);
            }

            return result;
        }
    }
}
