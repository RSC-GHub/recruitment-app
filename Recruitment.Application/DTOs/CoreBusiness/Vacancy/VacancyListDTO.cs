namespace Recruitment.Application.DTOs.CoreBusiness.Vacancy
{
    public class VacancyListDTO
    {
        public int Id { get; set; }
        public string TitleName { get; set; } = string.Empty;
        public int PositionCount { get; set; }
        public string EmploymentType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? Deadline { get; set; }

        public List<string> ProjectNames { get; set; } = new();


    }

}
