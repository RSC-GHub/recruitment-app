using Recruitment.Domain.Entities.Recruitment_Proccess;
using Recruitment.Domain.Enums;

namespace Recruitment.Domain.Entities.CoreBusiness
{
    public class Vacancy : BaseEntity
    {
        public int TitleId { get; set; }
        public Title? Title { get; set; }

        // Vacancy details
        public string JobDescription { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public string Responsibilities { get; set; } = string.Empty;
        public string Benefits { get; set; } = string.Empty;

        public int PositionCount { get; set; } = 1;
        public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;

        public decimal? SalaryRangeMin { get; set; }
        public decimal? SalaryRangeMax { get; set; }

        public VacancyStatus Status { get; set; } = VacancyStatus.Open;
        public DateTime? Deadline { get; set; }

        public ICollection<ProjectVacancy>? ProjectVacancies { get; set; }

        public ICollection<ApplicantApplication> Applications { get; set; } = new List<ApplicantApplication>();

    }
}
