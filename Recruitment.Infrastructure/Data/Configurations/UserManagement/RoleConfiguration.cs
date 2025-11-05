using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.UserManagement;

namespace Recruitment.Infrastructure.Data.Configurations.UserManagement
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            // Table name
            builder.ToTable("Roles");

            // Primary key
            builder.HasKey(r => r.Id);

            // Properties
            builder.Property(r => r.RoleName)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(r => r.RoleName)
                .IsUnique();

            builder.Property(r => r.Description)
                .HasMaxLength(500);

            builder.Property(r => r.IsActive)
                .HasDefaultValue(true);

            // Relationships
            builder.HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(r => r.RolePermissions)
                .WithOne(rp => rp.Role)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
