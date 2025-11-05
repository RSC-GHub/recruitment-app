using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.UserManagement;

namespace Recruitment.Infrastructure.Data.Configurations.UserManagement
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            // Table name
            builder.ToTable("RolePermissions");

            // Primary key
            builder.HasKey(rp => rp.Id);

            // Properties
            builder.Property(rp => rp.GrantedAt)
                .IsRequired();

            builder.Property(rp => rp.GrantedBy)
                .IsRequired();

            // Relationships
            builder.HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
