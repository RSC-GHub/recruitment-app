using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.RecruitmentProccess;

namespace Recruitment.Infrastructure.Data.Configurations.RecruitmentProcess
{
    public class RejectionReasonConfiguration : IEntityTypeConfiguration<RejectionReason>
    {
        public void Configure(EntityTypeBuilder<RejectionReason> builder)
        {
            builder.ToTable("RejectionReasons");
            builder.HasKey(rr => rr.Id);
            builder.Property(rr => rr.Reason)
                   .IsRequired()
                   .HasMaxLength(500);

            builder
          .HasMany(r => r.Interviews)
          .WithOne(ir => ir.RejectionReason)
          .HasForeignKey(ir => ir.RejectionReasonId);
        }
    }
}
