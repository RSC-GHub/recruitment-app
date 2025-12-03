using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.RecruitmentProccess.Interview
{
    public class UpdateInterviewDTO
    {
        public int Id { get; set; }  
        public string? InterViewer { get; set; }
        public InterviewStatus InterviewStatus { get; set; }
        public int DurationMinutes { get; set; }
        public string? InterviewNote { get; set; }
    }
}
