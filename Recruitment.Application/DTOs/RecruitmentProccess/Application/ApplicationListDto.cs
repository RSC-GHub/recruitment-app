using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.RecruitmentProccess.Application
{
    public class ApplicationListDto
    {
        public int Id { get; set; }

        // Applicant info
        public int ApplicantId { get; set; }
        public string ApplicantName { get; set; } = null!;
        public string ApplicantEmail { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        // Vacancy info
        public int VacancyId { get; set; }
        public string VacancyTitle { get; set; } = null!;

        public ApplicationStatus ApplicationStatus { get; set; }
        public DateTime ApplicationDate { get; set; }

        // Review info
        public string? ReviewedByUserName { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string? Note { get; set; }

        public DateTime? ExpectedFirstDate { get; set; }
        public DateTime? ActualFirstDate { get; set; }

    }
}
