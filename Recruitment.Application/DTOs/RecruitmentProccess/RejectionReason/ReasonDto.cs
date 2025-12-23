using Recruitment.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Recruitment.Application.DTOs.RecruitmentProccess.RejectionReason
{
    public class ReasonDto
    {
        public int Id { get; set; }
        [Required]
        public string Reason { get; set; } = string.Empty;
        [Required]
        public RejectionReasonType ReasonType { get; set; } 
    }
}
