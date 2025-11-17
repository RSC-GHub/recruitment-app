using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.CoreBusiness.Vacancy
{
    public class ProjectVacancyViewDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
    }

    public class VacancyViewDto
    {
        public int Id { get; set; }

        public int TitleId { get; set; }
        public string TitleName { get; set; } = string.Empty;

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

        public List<ProjectVacancyViewDto> Projects { get; set; } = new List<ProjectVacancyViewDto>();
    }
}
