using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.RecruitmentProccess.Application
{
    public class ApplicationReviewDto
    {
        public int ApplicationId { get; set; }
        public int ReviewedBy { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
        public string? Note { get; set; }
        public DateTime? ExpectedFirstDate { get; set; } 
        public DateTime? ActualFirstDate { get; set; } 

    }
}
