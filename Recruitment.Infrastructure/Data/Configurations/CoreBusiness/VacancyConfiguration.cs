using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Infrastructure.Data.Configurations.CoreBusiness
{
    public class VacancyConfiguration : IEntityTypeConfiguration<Vacancy>
    {
        public void Configure(EntityTypeBuilder<Vacancy> builder)
        {
            builder.ToTable("Vacancies");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.JobDescription).IsRequired();
            builder.Property(v => v.Requirements).IsRequired();
            builder.Property(v => v.Responsibilities).IsRequired();
            builder.Property(v => v.PositionCount).IsRequired(); 

            builder.Property(v => v.PositionCount)
                  .HasDefaultValue(1);

            builder.Property(v => v.EmploymentType)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.Property(v => v.Status)
                   .HasConversion<string>()
                   .HasMaxLength(20);

            builder.HasOne(v => v.Title)
                   .WithMany(t => t.Vacancies)
                   .HasForeignKey(v => v.TitleId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
