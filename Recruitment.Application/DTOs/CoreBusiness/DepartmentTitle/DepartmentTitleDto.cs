namespace Recruitment.Application.DTOs.CoreBusiness.DepartmentTitle
{
    public class DepartmentTitleDto
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int TitleId { get; set; }
        public string? TitleName { get; set; }
    }
}
