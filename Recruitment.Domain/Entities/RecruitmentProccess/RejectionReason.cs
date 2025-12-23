using Recruitment.Domain.Enums;

namespace Recruitment.Domain.Entities.RecruitmentProccess
{
    public class RejectionReason : BaseEntity
    {
        public string Reason { get; set; } = string.Empty;
        public RejectionReasonType ReasonType { get; set; } 
        public ICollection<InterviewRejectionReason> Interviews { get; set; } = new List<InterviewRejectionReason>();
        public ICollection<ApplicationRejectionReason> Applications { get; set; } = new List<ApplicationRejectionReason>();


    }
}
