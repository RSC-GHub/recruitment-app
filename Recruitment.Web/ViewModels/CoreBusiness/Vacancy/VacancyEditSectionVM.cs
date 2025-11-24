using Recruitment.Application.DTOs.CoreBusiness.Title;

namespace Recruitment.Web.ViewModels.CoreBusiness.Vacancy
{
    public class VacancyEditSectionVM
    {
        public int VacancyId { get; set; }
        public string TitleName { get; set; } 
        public string JobDescription { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public string Responsibilities { get; set; } = string.Empty;
        public string Benefits { get; set; } = string.Empty;

        public int PositionCount { get; set; }
        public string EmploymentType { get; set; } = string.Empty;
        public decimal? SalaryRangeMin { get; set; }
        public decimal? SalaryRangeMax { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? Deadline { get; set; }

        // Dropdown data
        public List<TitleDto> Titles { get; set; } = new();
        public List<ProjectSelectItemVM> AllProjects { get; set; } = new();

        // Selected projects for this vacancy
        public List<int> SelectedProjectIds { get; set; } = new();
    }
}
