using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.RecruitmentProccess;
using Recruitment.Domain.Enums;

namespace Recruitment.Infrastructure.Data.Configurations.RecruitmentProcess
{
    public class InterviewConfiguration : IEntityTypeConfiguration<Interview>
    {
        public void Configure(EntityTypeBuilder<Interview> builder)
        {
            builder.ToTable("Interviews");

            // Primary Key
            builder.HasKey(i => i.Id);

            // Relationships
            builder.HasOne(i => i.Application)
                   .WithMany(a => a.Interviews)
                   .HasForeignKey(i => i.ApplicationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(i => i.ScheduledDate)
                   .IsRequired();

            builder.Property(i => i.DurationMinutes)
                   .IsRequired();

            // Enums stored as int
            builder.Property(i => i.InterviewType)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(i => i.InterviewCategory)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(i => i.InterviewStatus)
                   .HasConversion<int>()
                   .HasDefaultValue(InterviewStatus.Scheduled)
                   .IsRequired();

            builder.Property(i => i.InterviewResult)
                   .HasConversion<int>()
                   .HasDefaultValue(InterviewResult.Pending)
                   .IsRequired();

            builder.Property(i => i.Feedback)
                   .HasMaxLength(2000)
                   .IsUnicode()
                   .IsRequired(false);

            builder.HasMany(i => i.RejectionReasons)
                   .WithOne(ir => ir.Interview)
                   .HasForeignKey(ir => ir.InterviewId);
        }
    }
}
