namespace Recruitment.Application.DTOs.Reports
{
    public class ReportDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string StoredProcedure { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public List<ReportParameterDto> Parameters { get; set; } = new();

    }
}
