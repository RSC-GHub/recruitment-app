using Recruitment.Domain.Enums;

namespace Recruitment.Domain.Entities.CoreBusiness
{
    public class Project : BaseEntity
    {
        public string ProjectName { get; set; } = string.Empty;
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;
      
        public int LocationId { get; set; }
        public Location? Location { get; set; }

        public ICollection<ProjectVacancy>? ProjectVacancies { get; set; }
        public ICollection<UserProject>? UserProjects { get; set; } = new List<UserProject>();

    }
}
