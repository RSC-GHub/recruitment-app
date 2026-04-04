using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.UserManagement;

namespace Recruitment.Infrastructure.Data.Configurations.UserManagement
{
    public class ApplicantConfiguration : IEntityTypeConfiguration<Applicant>
    {
        public void Configure(EntityTypeBuilder<Applicant> builder)
        {
            builder.ToTable("Applicants");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Required Fields
            builder.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.PhoneNumber)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Nationality)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.CurrentJob)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.CurrentEmployer)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.CurrentSalary)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.ExpectedSalary)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.NoticePeriod)
                .HasMaxLength(150);

            builder.Property(x => x.Major)
                .HasMaxLength(150);

            builder.Property(x => x.Address)
                .HasMaxLength(300);

            builder.Property(x => x.ExtraCertificate)
                .HasMaxLength(300);

            // Enums
            builder.Property(x => x.Gender)
                .IsRequired();

            builder.Property(x => x.MilitaryStatus)
                .IsRequired();

            builder.Property(x => x.MaritalStatus)
                .IsRequired();

            builder.Property(x => x.EducationDegree)
                .IsRequired();

            // Graduation year
            builder.Property(x => x.GraduationYear)
                .HasColumnType("smallint");

            // CV File Path
            builder.Property(x => x.CVFilePath)
                .HasMaxLength(500)
                .IsRequired();

            // Relationships
            builder.HasOne(x => x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Currency)
                .WithMany()
                .HasForeignKey(x => x.CurrencyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.MasterApplicant)
                .WithMany(d => d.Duplicates)
                .HasForeignKey(x => x.MasterApplicantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
