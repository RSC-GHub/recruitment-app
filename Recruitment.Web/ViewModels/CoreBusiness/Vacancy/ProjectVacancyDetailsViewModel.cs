using Recruitment.Domain.Enums;

namespace Recruitment.Web.ViewModels.CoreBusiness.Vacancy
{
    public class ProjectVacancyDetailsViewModel
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public PriorityLevel Priority { get; set; }
    }

    public class VacancyDetailsViewModel
    {
        public int Id { get; set; }

        public int TitleId { get; set; }
        public string TitleName { get; set; } = string.Empty;

        public string JobDescription { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public string Responsibilities { get; set; } = string.Empty;
        public string Benefits { get; set; } = string.Empty;

        public int PositionCount { get; set; }
        public EmploymentType EmploymentType { get; set; }

        public decimal? SalaryRangeMin { get; set; }
        public decimal? SalaryRangeMax { get; set; }

        public VacancyStatus Status { get; set; }
        public DateTime? Deadline { get; set; }

        public List<ProjectVacancyDetailsViewModel> Projects { get; set; }
            = new List<ProjectVacancyDetailsViewModel>();
    }
}
