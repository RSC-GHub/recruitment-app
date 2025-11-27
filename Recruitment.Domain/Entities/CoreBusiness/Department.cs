using Recruitment.Domain.Entities.UserManagement;

namespace Recruitment.Domain.Entities.CoreBusiness
{
    public class Department : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<DepartmentTitle>? DepartmentTitles { get; set; }
        public ICollection<User>? Users { get; set; }
    }
}
