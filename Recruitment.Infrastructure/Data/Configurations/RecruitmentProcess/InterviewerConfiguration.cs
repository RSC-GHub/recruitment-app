using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.RecruitmentProccess;

namespace Recruitment.Infrastructure.Data.Configurations.RecruitmentProcess
{
    public class InterviewerConfiguration : IEntityTypeConfiguration<Interviewer>
    {
        public void Configure(EntityTypeBuilder<Interviewer> builder)
        {

            builder.ToTable("Interviewers");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Name)
                   .IsRequired()
                   .HasMaxLength(150);


            // Interviewer → Department (Many-to-One)
            builder.HasOne(i => i.Department)
                   .WithMany(d => d.Interviewers)
                   .HasForeignKey(i => i.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Interviewer → Interviews (One-to-Many)
            builder.HasMany(i => i.Interviews)
                   .WithOne(interview => interview.Interviewer)
                   .HasForeignKey(interview => interview.InterviewerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
