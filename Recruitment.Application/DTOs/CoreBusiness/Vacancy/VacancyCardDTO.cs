namespace Recruitment.Application.DTOs.CoreBusiness.Vacancy
{
    public class VacancyCardDTO
    {
        public int Id { get; set; }
        public string TitleName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string EmploymentType { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string KeyRequirements { get; set; } = string.Empty; 
    }
}
