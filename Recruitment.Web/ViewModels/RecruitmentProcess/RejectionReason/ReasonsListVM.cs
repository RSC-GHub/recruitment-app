using Recruitment.Domain.Enums;

namespace Recruitment.Web.ViewModels.RecruitmentProcess.RejectionReason
{
    public class ReasonsListVM
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public RejectionReasonType ReasonType { get; set; }

    }
}
