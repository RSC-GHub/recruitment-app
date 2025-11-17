using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.CoreBusiness.Vacancy
{
    public class VacancyUpdateDto
    {
        public int Id { get; set; }  

        public int TitleId { get; set; }

        public string JobDescription { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public string Responsibilities { get; set; } = string.Empty;
        public string Benefits { get; set; } = string.Empty;

        public int PositionCount { get; set; } = 1;
        public EmploymentType EmploymentType { get; set; } = EmploymentType.FullTime;

        public decimal? SalaryRangeMin { get; set; }
        public decimal? SalaryRangeMax { get; set; }

        public DateTime? Deadline { get; set; }

        public List<ProjectVacancyDto> Projects { get; set; } = new List<ProjectVacancyDto>();
    }
}
