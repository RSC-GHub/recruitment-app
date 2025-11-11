using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.CoreBusiness.Vacancy
{
    public class ProjectPriorityDto
    {
        public int ProjectId { get; set; }
        public string? ProjectName { get; set; } 
        public PriorityLevel Priority { get; set; }
    }
    public class VacancyCreateDto
    {
        public string JobDescription { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public string Responsibilities { get; set; } = string.Empty;
        public string Benefits { get; set; } = string.Empty;

        public int PositionCount { get; set; } 
        public int TitleId { get; set; }
        public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;
        public decimal? SalaryRangeMin { get; set; }
        public decimal? SalaryRangeMax { get; set; }
        public VacancyStatus Status { get; set; } = VacancyStatus.Open;
        public DateTime? Deadline { get; set; }

        public List<int> ProjectIds { get; set; } = new();

        public List<ProjectPriorityDto>? Projects { get; set; }

    }
}
