using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.RecruitmentProccess.Interview
{
    public class UpdateInterviewResultDTO
    {
        public int Id { get; set; }
        public InterviewResult InterviewResult { get; set; }
        public string? Feedback { get; set; }
        public string? Note { get; set; }

        public List<int>? RejectionReasonIds { get; set; } = new List<int>();

    }

}
