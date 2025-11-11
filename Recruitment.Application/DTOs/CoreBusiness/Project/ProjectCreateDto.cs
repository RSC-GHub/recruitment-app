using Recruitment.Domain.Enums;

namespace Recruitment.Application.DTOs.CoreBusiness.Project
{
    public class ProjectCreateDto
    {
        public string ProjectName { get; set; } = string.Empty;
        public ProjectStatus Status { get; set; }
        public int LocationId { get; set; }
    }
}
