using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Infrastructure.Data.Configurations.CoreBusiness
{
    public class DepartmentTitleConfiguration : IEntityTypeConfiguration<DepartmentTitle>
    {
        public void Configure(EntityTypeBuilder<DepartmentTitle> builder)
        {
            builder.ToTable("DepartmentTitles");

            builder.HasKey(dt => dt.Id);

            builder.HasOne(dt => dt.Department)
                   .WithMany(d => d.DepartmentTitles)
                   .HasForeignKey(dt => dt.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(dt => dt.Title)
                   .WithMany(t => t.DepartmentTitles)
                   .HasForeignKey(dt => dt.TitleId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
