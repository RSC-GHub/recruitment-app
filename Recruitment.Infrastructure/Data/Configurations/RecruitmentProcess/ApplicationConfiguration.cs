using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.Recruitment_Proccess;


namespace Recruitment.Infrastructure.Data.Configurations.Recruitment_Process
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<ApplicantApplication>
    {
        public void Configure(EntityTypeBuilder<ApplicantApplication> builder)
        {
            // Table name
            builder.ToTable("Applications");

            // Primary key (from BaseEntity)
            builder.HasKey(a => a.Id);

            // Relationships

            // Applicant
            builder.HasOne(a => a.Applicant)
                   .WithMany(app => app.Applications) 
                   .HasForeignKey(a => a.ApplicantId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Vacancy
            builder.HasOne(a => a.Vacancy)
                   .WithMany(v => v.Applications) 
                   .HasForeignKey(a => a.VacancyId)
                   .OnDelete(DeleteBehavior.Cascade);

            // في ApplicationConfiguration
            builder.HasOne(a => a.Reviewer)
                   .WithMany(u => u.ReviewedApplications) 
                   .HasForeignKey(a => a.ReviewedBy)
                   .OnDelete(DeleteBehavior.SetNull);


            // Properties

            builder.Property(a => a.ApplicationDate)
                   .IsRequired();

            builder.Property(a => a.ReviewDate)
                   .IsRequired(false);

            builder.Property(a => a.ApplicationStatus)
                   .IsRequired();

            builder.Property(a => a.Note)
                   .HasMaxLength(1000)
                   .IsRequired(false);
        }
    }
}

