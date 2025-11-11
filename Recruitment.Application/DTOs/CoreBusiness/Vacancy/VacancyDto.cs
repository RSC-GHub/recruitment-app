using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.CoreBusiness.Vacancy
{
    public class VacancyDto : ProjectPriorityDto
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

        public List<ProjectPriorityDto> Projects { get; set; } = new();
        public List<int> ProjectIds => Projects.Select(p => p.ProjectId).ToList();
        public List<string> ProjectNames => Projects.Select(p => p.ProjectName ?? string.Empty).ToList();
    }

}
