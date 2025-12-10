using Recruitment.Domain.Enums;

namespace Recruitment.Web.ViewModels.RecruitmentProcess.Application
{
    public class ApplicationDetailVM
    {
        public int Id { get; set; }

        public int ApplicantId { get; set; }
        public string ApplicantName { get; set; } = "";
        public string ApplicantEmail { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string? CurrentJob { get; set; }
        public string? CurrentEmployer { get; set; }

        public int VacancyId { get; set; }
        public string VacancyTitle { get; set; } = "";
        public string? VacancyDescription { get; set; }

        public ApplicationStatus ApplicationStatus { get; set; }
        public DateTime ApplicationDate { get; set; }

        public int? ReviewedBy { get; set; }
        public string? ReviewedByUserName { get; set; }
        public DateTime? ReviewDate { get; set; }
        public string? Note { get; set; }
        public bool HasFirstInterview { get; set; }
        public string CVFilePath { get; set; } = null!;

    }
}
