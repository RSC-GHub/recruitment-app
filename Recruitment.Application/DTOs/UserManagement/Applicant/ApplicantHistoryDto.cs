using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.UserManagement.Applicant
{
    public class ApplicantHistoryDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CountryName { get; set; }
        public string? CityName { get; set; }
        public string? Comment { get; set; } = string.Empty;

        public List<ApplicationHistoryDto> Applications { get; set; } = new();
    }

    public class ApplicationHistoryDto
    {
        public int Id { get; set; }
        public string VacancyTitle { get; set; } = "";
        public ApplicationStatus ApplicationStatus { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ReviewedByUserName { get; set; } = "-";
        public string? Note { get; set; }
        public DateTime? ExpectedFirstDate { get; set; }
        public DateTime? ActualFirstDate { get; set; }
        public List<InterviewHistoryDto> Interviews { get; set; } = new();
    }

    public class InterviewHistoryDto
    {
        public int Id { get; set; }
        public string? InterviewerName { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string InterviewType { get; set; } = "";
        public string InterviewCategory { get; set; } = "";
        public InterviewStatus InterviewStatus { get; set; }
        public string InterviewResult { get; set; } = "";
        public string? Feedback { get; set; }
        public string? InterViewNote { get; set; }
    }
}
