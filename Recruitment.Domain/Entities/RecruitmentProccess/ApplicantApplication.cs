using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Entities.UserManagement;
using Recruitment.Domain.Enums;

namespace Recruitment.Domain.Entities.Recruitment_Proccess
{
    public class ApplicantApplication : BaseEntity
    {
        public int VacancyId { get; set; }
        public Vacancy Vacancy { get; set; } = null!;

        public int ApplicantId { get; set; }
        public Applicant Applicant { get; set; } = null!;

        public int? ReviewedBy { get; set; }
        public User? Reviewer { get; set; }

        public DateTime? ReviewDate { get; set; }
        public DateTime ApplicationDate { get; set; } = DateTime.Now;

        public ApplicationStatus ApplicationStatus { get; set; } = ApplicationStatus.Submitted;

        public string? Note { get; set; }
    }
}