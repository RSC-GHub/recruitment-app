using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.UserManagement;

namespace Recruitment.Infrastructure.Data.Configurations.UserManagement
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table name
            builder.ToTable("AspNetUsers");

            // Primary key
            builder.HasKey(u => u.Id);

            // Properties
            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.IsActive)
                .HasDefaultValue(true);

            builder.Property(u => u.LastLogin)
                .IsRequired(false);

            // Relationships

            builder.HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
