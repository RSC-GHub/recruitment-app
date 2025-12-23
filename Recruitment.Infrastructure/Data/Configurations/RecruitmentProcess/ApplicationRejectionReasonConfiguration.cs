using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.RecruitmentProccess;

namespace Recruitment.Infrastructure.Data.Configurations.RecruitmentProcess
{
    public class ApplicationRejectionReasonConfiguration : IEntityTypeConfiguration<ApplicationRejectionReason>
    {
        public void Configure(EntityTypeBuilder<ApplicationRejectionReason> builder)
        {
            builder.HasKey(ar => new { ar.ApplicationId, ar.RejectionReasonId });

            builder.HasOne(ar => ar.Application)
                   .WithMany(a => a.RejectionReasons) 
                   .HasForeignKey(ar => ar.ApplicationId);

            builder.HasOne(ar => ar.RejectionReason)
                   .WithMany(r => r.Applications) 
                   .HasForeignKey(ar => ar.RejectionReasonId);
        }
    }
}
