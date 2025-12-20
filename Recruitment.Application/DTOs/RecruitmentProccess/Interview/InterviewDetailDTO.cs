using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.RecruitmentProccess.Interview
{
    public class InterviewDetailDTO
    {
        public int Id { get; set; }

        // Applicant & Vacancy info
        public int ApplicationId { get; set; }

        public int ApplicantId { get; set; }
        public string ApplicantName { get; set; } = null!;
        public string ApplicantEmail { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!; 
        public string VacancyTitle { get; set; } = null!;
        public EmploymentType EmploymentType { get; set; }
        public List<string> VacancyProjects { get; set; } = new();


        // Interview info
        public int InterviewerId { get; set; }
        public string? InterviewerName { get; set; } 
        public DateTime ScheduledDate { get; set; }
        public InterviewType InterviewType { get; set; }
        public InterviewCategory InterviewCategory { get; set; }
        public InterviewStatus InterviewStatus { get; set; }
        public InterviewResult InterviewResult { get; set; }
        public string? Feedback { get; set; }
        public int DurationMinutes { get; set; }
        public string? InterViewNote { get; set; }

        // Audit Info
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }

}
