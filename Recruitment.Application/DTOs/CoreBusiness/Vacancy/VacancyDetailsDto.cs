using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.CoreBusiness.Vacancy
{
    public class VacancyDetailsDto
    {
        public int Id { get; set; }
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

        public List<ProjectSummaryDto> Projects { get; set; } = new();
    }
    public class ProjectSummaryDto
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
    }
}
