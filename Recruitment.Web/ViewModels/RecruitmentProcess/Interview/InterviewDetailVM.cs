using Recruitment.Domain.Enums;
using System.Security.Principal;

namespace Recruitment.Web.ViewModels.RecruitmentProcess.Interview
{
    public class InterviewDetailVM
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int ApplicantId { get; set; }
        public string ApplicantName { get; set; } = ""; 
        public string ApplicantEmail { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string VacancyTitle { get; set; } = "";
        public EmploymentType EmploymentType { get; set; }
        public List<string> VacancyProjects { get; set; } = new();

        public string? InterViewer { get; set; }
        public DateTime ScheduledDate { get; set; }
        public InterviewType InterviewType { get; set; }
        public InterviewCategory InterviewCategory { get; set; }
        public InterviewStatus InterviewStatus { get; set; }
        public InterviewResult InterviewResult { get; set; }
        public string? Feedback { get; set; }
        public int DurationMinutes { get; set; }
        public string? InterViewNote { get; set; }

        // Audit info
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

}

