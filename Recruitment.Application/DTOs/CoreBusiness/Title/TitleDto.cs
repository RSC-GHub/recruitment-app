namespace Recruitment.Application.DTOs.CoreBusiness.Title
{
    public class TitleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public List<int> DepartmentIds { get; set; } = new();
        public List<string> DepartmentNames { get; set; } = new();
    }

}
