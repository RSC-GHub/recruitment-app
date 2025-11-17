using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Domain.Enums;

namespace Recruitment.Web.ViewModels.CoreBusiness.Vacancy
{
    public class ProjectVacancyFormViewModel
    {
        public int ProjectId { get; set; }
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
    }
    public class VacancyFormViewModel
    {
        public int Id { get; set; }  // 0 = Create

        public int TitleId { get; set; }
        public List<SelectListItem>? Titles { get; set; }

        public string JobDescription { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public string Responsibilities { get; set; } = string.Empty;
        public string Benefits { get; set; } = string.Empty;

        public int PositionCount { get; set; } = 1;
        public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;

        public decimal? SalaryRangeMin { get; set; }
        public decimal? SalaryRangeMax { get; set; }

        public DateTime? Deadline { get; set; }

        public List<ProjectVacancyFormViewModel> Projects { get; set; }
            = new List<ProjectVacancyFormViewModel>();

        // Project List for Dropdown
        public List<SelectListItem>? ProjectList { get; set; }
    }
}
