namespace Recruitment.Application.DTOs.Reports
{
    public class CreateReportDto
    {
        public string Name { get; set; } = null!;
        public string StoredProcedure { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public List<CreateReportParameterDto> Parameters { get; set; } = new();
    }

}
