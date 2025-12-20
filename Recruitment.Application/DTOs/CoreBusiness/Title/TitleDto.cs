using Recruitment.Application.DTOs.CoreBusiness.Department;

namespace Recruitment.Application.DTOs.CoreBusiness.Title
{
    public class TitleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<DepartmentDto> Departments { get; set; } = new();
    }
}
