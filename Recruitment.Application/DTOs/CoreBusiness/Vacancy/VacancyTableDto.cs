using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.CoreBusiness.Vacancy
{
    public class ProjectVacancyTableDto
    {
        public string ProjectName { get; set; } = string.Empty;
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
    }
    public class VacancyTableDto
    {
        public int Id { get; set; }

        public string TitleName { get; set; } = string.Empty;
        public int PositionCount { get; set; } = 1;
        public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;
        public VacancyStatus Status { get; set; } = VacancyStatus.Open;
        public List<ProjectVacancyTableDto> Projects { get; set; } = new List<ProjectVacancyTableDto>();
    }
}
