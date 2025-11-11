using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.UserManagement;

namespace Recruitment.Infrastructure.Data.Configurations.UserManagement
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            // Table name
            builder.ToTable("Permissions");

            // Primary key
            builder.HasKey(p => p.Id);

            // Properties
            builder.Property(p => p.PermissionName)
                .IsRequired()
                .HasMaxLength(100);

            //builder.HasIndex(p => p.PermissionName)
            //    .IsUnique();

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.Resource)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.Action)
                .IsRequired()
                .HasMaxLength(50);

            // Relationships
            builder.HasMany(p => p.RolePermissions)
                .WithOne(rp => rp.Permission)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
