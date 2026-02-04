namespace Recruitment.Web.ViewModels.Report
{
    public class ReportListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string StoredProcedure { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

}
