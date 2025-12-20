using Recruitment.Domain.Enums;

namespace Recruitment.Web.ViewModels.UserManagement.Applicant
{
    // InterviewHistoryVM.cs
    public class InterviewHistoryVM
    {
        public int Id { get; set; }
        public string InterviewerName { get; set; } = "-";
        public DateTime ScheduledDate { get; set; }
        public string InterviewType { get; set; } = "-";
        public string InterviewCategory { get; set; } = "-";
        public InterviewStatus InterviewStatus { get; set; }
        public string InterviewResult { get; set; } = "-";
        public string? Feedback { get; set; }
        public string? InterViewNote { get; set; }
    }

    // ApplicationHistoryVM.cs
    public class ApplicationHistoryVM
    {
        public int Id { get; set; }
        public string VacancyTitle { get; set; } = "-";
        public ApplicationStatus ApplicationStatus { get; set; }
        public DateTime ApplicationDate { get; set; }
        public string ReviewedByUserName { get; set; } = "-";
        public string? Note { get; set; }
        public List<InterviewHistoryVM> Interviews { get; set; } = new();
    }

    // ApplicantHistoryVM.cs
    public class ApplicantHistoryVM
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "-";
        public string Email { get; set; } = "-";
        public string PhoneNumber { get; set; } = "-";
        public string? CountryName { get; set; }
        public string? CityName { get; set; }
        public List<ApplicationHistoryVM> Applications { get; set; } = new();
    }

}
