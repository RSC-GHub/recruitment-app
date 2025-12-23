using Recruitment.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.RecruitmentProcess.RejectionReason
{
    public class CreateRejectionReasonVM
    {
        [Required]
        public string Reason { get; set; }
        public RejectionReasonType ReasonType { get; set; }

    }
}
