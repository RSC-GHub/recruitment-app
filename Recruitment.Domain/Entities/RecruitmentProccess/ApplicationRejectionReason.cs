using Recruitment.Domain.Entities.Recruitment_Proccess;

namespace Recruitment.Domain.Entities.RecruitmentProccess
{
    public class ApplicationRejectionReason : BaseEntity
    {
        public int ApplicationId { get; set; }
        public ApplicantApplication Application { get; set; } = null!;

        public int RejectionReasonId { get; set; }
        public RejectionReason RejectionReason { get; set; } = null!;
    }

}
