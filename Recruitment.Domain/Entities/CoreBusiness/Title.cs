namespace Recruitment.Domain.Entities.CoreBusiness
{
    public class Title : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<DepartmentTitle>? DepartmentTitles { get; set; }
        // public ICollection<Vacancy>? Vacancies { get; set; }
    }
}
