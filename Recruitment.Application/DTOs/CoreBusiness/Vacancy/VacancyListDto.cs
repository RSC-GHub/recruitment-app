using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.CoreBusiness.Vacancy
{
    public class VacancyListDto
    {
        public int Id { get; set; }
        public string TitleName { get; set; } = string.Empty;

        public int PositionCount { get; set; }
        public EmploymentType EmploymentType { get; set; }
        public VacancyStatus Status { get; set; }

        public List<string> ProjectNames { get; set; } = new();
    }
}
