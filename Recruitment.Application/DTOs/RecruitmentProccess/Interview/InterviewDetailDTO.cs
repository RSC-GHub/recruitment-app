using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.RecruitmentProccess.Interview
{
    public class InterviewDetailDTO
    {
        public int Id { get; set; }

        // Applicant & Vacancy info
        public int ApplicationId { get; set; }
        public string ApplicantName { get; set; } = null!;
        public string ApplicantEmail { get; set; } = null!;
        public string VacancyTitle { get; set; } = null!;

        // Interview info
        public string? InterViewer { get; set; }
        public DateTime ScheduledDate { get; set; }
        public InterviewType InterviewType { get; set; }
        public InterviewStatus InterviewStatus { get; set; }
        public InterviewResult InterviewResult { get; set; }
        public string? Feedback { get; set; }
        public int DurationMinutes { get; set; }
        public string? InterViewNote { get; set; }
    }
}
