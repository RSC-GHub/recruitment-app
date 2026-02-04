namespace Recruitment.Domain.Entities.Reports
{
    public class Report : BaseEntity
    {
        public string Name { get; set; } = null!;          
        public string StoredProcedure { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ReportParameter> Parameters { get; set; } = new List<ReportParameter>();

    }
}
