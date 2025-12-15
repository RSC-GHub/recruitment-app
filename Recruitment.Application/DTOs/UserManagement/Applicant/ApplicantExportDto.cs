using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.UserManagement.Applicant
{
    public class ApplicantExportDto
    {
        public string ApplicantName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Major { get; set; }
        public int? GraduationYear { get; set; }

        public string Position { get; set; }
        public string CurrentPosition { get; set; }
        public string CurrentCompany { get; set; }

        public string Projects { get; set; }
        public string Departments { get; set; }

        public InterviewResult? HRResult { get; set; }
        public InterviewResult? TechResult { get; set; }

        public decimal? CurrentSalary { get; set; }
        public decimal? ExpectedSalary { get; set; }
        public int? NoticePeriod { get; set; }

        public string Address { get; set; }

        public DateTime? HRInterviewDate { get; set; }
        public DateTime? TechInterviewDate { get; set; }

        public string RecruiterName { get; set; }

        public string HRNote { get; set; }
        public string TechNote { get; set; }

        public string HRInterviewer { get; set; }
        public string TechInterviewer { get; set; }

        public string CV { get; set; }
    }

}
