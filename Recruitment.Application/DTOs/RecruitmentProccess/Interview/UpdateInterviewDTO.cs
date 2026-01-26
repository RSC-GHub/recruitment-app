using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.RecruitmentProccess.Interview
{
    public class UpdateInterviewDTO
    {
        public int Id { get; set; }  
        public int InterviewerId { get; set; }
        public InterviewStatus InterviewStatus { get; set; }
        public InterviewType InterviewType { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public string? InterviewNote { get; set; }
    }
}
