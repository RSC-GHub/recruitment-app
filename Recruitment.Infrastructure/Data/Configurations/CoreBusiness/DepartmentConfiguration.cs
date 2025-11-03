using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Infrastructure.Data.Configurations.CoreBusiness
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasMany(d => d.DepartmentTitles)
                   .WithOne(dt => dt.Department)
                   .HasForeignKey(dt => dt.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
