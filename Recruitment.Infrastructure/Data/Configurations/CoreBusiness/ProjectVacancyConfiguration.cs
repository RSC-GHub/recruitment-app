using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Infrastructure.Data.Configurations.CoreBusiness
{
    public class ProjectVacancyConfiguration : IEntityTypeConfiguration<ProjectVacancy>
    {
        public void Configure(EntityTypeBuilder<ProjectVacancy> builder)
        {
            builder.ToTable("ProjectVacancies");

            builder.HasKey(pv => pv.Id);

            builder.Property(pv => pv.Priority)
                   .HasConversion<string>()
                   .IsRequired()
                   .HasMaxLength(20);


            builder.HasOne(pv => pv.Project)
                   .WithMany(p => p.ProjectVacancies)
                   .HasForeignKey(pv => pv.ProjectId)
                   .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(pv => pv.Vacancy)
                   .WithMany(v => v.ProjectVacancies)
                   .HasForeignKey(pv => pv.VacancyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
