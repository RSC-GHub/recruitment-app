using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.RecruitmentProccess.Application
{
    public class ApplicationDetailDto
    {
        public int Id { get; set; }

        // Applicant details
        public int ApplicantId { get; set; }
        public string ApplicantName { get; set; } = null!;
        public string ApplicantEmail { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public string? TargetPosition { get; set; }
        public string? CurrentJob { get; set; }
        public string? CurrentEmployer { get; set; }
        public string CV { get; set; } = null!;
         
        // Vacancy details
        public int VacancyId { get; set; }
        public string VacancyTitle { get; set; } = null!;
        public string? VacancyDescription { get; set; }

        public ApplicationStatus ApplicationStatus { get; set; }
        public DateTime ApplicationDate { get; set; }

        // Review info
        public int? ReviewedBy { get; set; }
        public string? ReviewedByUserName { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string? Note { get; set; }

        public DateTime? ExpectedFirstDate { get; set; }
        public DateTime? ActualFirstDate { get; set; }

        public List<int> RejectionReasonIds { get; set; } = new();
        public List<string> RejectionReasonTexts { get; set; } = new();


    }
}
