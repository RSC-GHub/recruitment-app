using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.RecruitmentProccess.Interview
{
    public class InterviewCreateUpdateDTO
    {
        public int ApplicationId { get; set; }
        public string? Interviewer { get; set; }
        public DateTime ScheduledDate { get; set; }
        public InterviewType InterviewType { get; set; }
        public int DurationMinutes { get; set; }

        // Optional update fields
        public InterviewStatus? InterviewStatus { get; set; }
        public InterviewResult? InterviewResult { get; set; }
        public string? Feedback { get; set; }
        public string? InterViewNote { get; set; }
    }
}
