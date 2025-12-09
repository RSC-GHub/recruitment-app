namespace Recruitment.Application.DTOs.Audit
{
    public class RecentActivityDto
    {
        public string Icon { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
