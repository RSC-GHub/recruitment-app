namespace Recruitment.Domain.Entities.CoreBusiness
{
    public class Department : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<DepartmentTitle>? DepartmentTitles { get; set; }
    }
}
