using Recruitment.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Recruitment.Application.DTOs.RecruitmentProccess.RejectionReason
{
    public class CreateReasonDto
    {
        [Required]
        public string Reason { get; set; } = string.Empty;
        public RejectionReasonType ReasonType { get; set; }

    }
}
