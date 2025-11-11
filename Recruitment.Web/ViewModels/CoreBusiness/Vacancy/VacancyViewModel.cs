using Recruitment.Domain.Enums;

namespace Recruitment.Web.ViewModels.CoreBusiness.Vacancy
{
    public class VacancyViewModel
    {
        public int Id { get; set; }
        public string JobDescription { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public string Responsibilities { get; set; } = string.Empty;
        public string Benefits { get; set; } = string.Empty;
        public int PositionCount { get; set; }
        public int TitleId { get; set; }
        public string? TitleName { get; set; }
        public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;
        public decimal? SalaryRangeMin { get; set; }
        public decimal? SalaryRangeMax { get; set; }
        public VacancyStatus Status { get; set; } = VacancyStatus.Open;
        public DateTime? Deadline { get; set; }

        public List<int> ProjectIds { get; set; } = new();
        public List<string>? ProjectNames { get; set; }
        public List<PriorityLevel>? ProjectPriorities { get; set; } = new();

    }
}
