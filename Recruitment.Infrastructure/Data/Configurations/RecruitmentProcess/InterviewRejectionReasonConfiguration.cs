using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.RecruitmentProccess;

namespace Recruitment.Infrastructure.Data.Configurations.RecruitmentProcess
{
    public class InterviewRejectionReasonConfiguration : IEntityTypeConfiguration<InterviewRejectionReason>
    {
        public void Configure(EntityTypeBuilder<InterviewRejectionReason> builder)
        {
            builder.HasKey(ir => new { ir.InterviewId, ir.RejectionReasonId });

            builder.HasOne(ir => ir.Interview)
                   .WithMany(i => i.RejectionReasons)
                   .HasForeignKey(ir => ir.InterviewId);

            builder.HasOne(ir => ir.RejectionReason)
                   .WithMany(r => r.Interviews)
                   .HasForeignKey(ir => ir.RejectionReasonId);
        }
    }
}
