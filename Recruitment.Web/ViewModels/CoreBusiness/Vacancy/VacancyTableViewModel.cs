using Recruitment.Domain.Enums;

namespace Recruitment.Web.ViewModels.CoreBusiness.Vacancy
{
    public class ProjectVacancyTableViewModel
    {
        public string ProjectName { get; set; } = string.Empty;
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
    }

    public class VacancyTableViewModel
    {
        public int Id { get; set; }
        public string TitleName { get; set; } = string.Empty;

        public int PositionCount { get; set; }
        public EmploymentType EmploymentType { get; set; }
        public VacancyStatus Status { get; set; }

        public List<ProjectVacancyTableViewModel> Projects { get; set; }
            = new List<ProjectVacancyTableViewModel>();
    }
}
