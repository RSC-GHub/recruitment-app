using Recruitment.Domain.Enums;

namespace Recruitment.Web.ViewModels.CoreBusiness.Project
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public ProjectStatus Status { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
    }
}
