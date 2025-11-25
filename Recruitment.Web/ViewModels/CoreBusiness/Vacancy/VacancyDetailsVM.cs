namespace Recruitment.Web.ViewModels.CoreBusiness.Vacancy
{
    public class VacancyDetailsVM
    {
        public int Id { get; set; }
        public string TitleName { get; set; } = string.Empty;
        public string JobDescription { get; set; } = string.Empty;
        public string Requirements { get; set; } = string.Empty;
        public string Responsibilities { get; set; } = string.Empty;
        public string Benefits { get; set; } = string.Empty;
        public int PositionCount { get; set; }
        public string EmploymentType { get; set; } = string.Empty;
        public decimal? SalaryRangeMin { get; set; }
        public decimal? SalaryRangeMax { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? Deadline { get; set; }
        public List<string> ProjectNames { get; set; } = new();
    }
}
