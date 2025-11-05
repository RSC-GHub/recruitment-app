namespace Recruitment.Application.DTOs.CoreBusiness.Title
{
    public class CreateTitleDto
    {
        public string Name { get; set; } = string.Empty;
        public List<int> DepartmentIds { get; set; } = new();

    }
}
