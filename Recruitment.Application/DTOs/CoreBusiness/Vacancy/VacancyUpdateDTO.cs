using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.CoreBusiness.Vacancy
{
    public class VacancyUpdateDTO
    {
        public int Id { get; set; }

        public int TitleId { get; set; }

        public string JobDescription { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public string Responsibilities { get; set; } = string.Empty;
        public string Benefits { get; set; } = string.Empty;

        public int PositionCount { get; set; } = 1;
        public EmploymentType EmploymentType { get; set; }

        public decimal? SalaryRangeMin { get; set; }
        public decimal? SalaryRangeMax { get; set; }

        public VacancyStatus Status { get; set; }
        public DateTime? Deadline { get; set; }

        public List<int> ProjectIds { get; set; } = new();
    }

}
