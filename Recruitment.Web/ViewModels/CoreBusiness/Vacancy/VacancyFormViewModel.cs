using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Domain.Enums;

namespace Recruitment.Web.ViewModels.CoreBusiness.Vacancy
{
    public class ProjectWithPriority
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
    }

    public class VacancyFormViewModel
    {
        public int Id { get; set; }
        public int TitleId { get; set; }
        public string JobDescription { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public string Responsibilities { get; set; } = string.Empty;
        public string Benefits { get; set; } = string.Empty;
        public int PositionCount { get; set; }
        public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;
        public decimal? SalaryRangeMin { get; set; }
        public decimal? SalaryRangeMax { get; set; }
        public VacancyStatus Status { get; set; } = VacancyStatus.Open;
        public DateTime? Deadline { get; set; }

        public IEnumerable<SelectListItem>? Titles { get; set; }
        public List<ProjectWithPriority> ProjectsWithPriority { get; set; } = new();

        public IEnumerable<SelectListItem> PriorityLevels => Enum.GetValues(typeof(PriorityLevel))
            .Cast<PriorityLevel>()
            .Select(p => new SelectListItem
            {
                Text = p.ToString(),
                Value = p.ToString()
            });
    }
}
