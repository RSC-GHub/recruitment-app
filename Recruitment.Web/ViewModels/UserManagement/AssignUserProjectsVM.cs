using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Web.ViewModels.UserManagement
{
    public class AssignUserProjectsVM
    {
        public int UserId { get; set; }

        public List<int> SelectedProjectIds { get; set; } = new();

        public List<Project> AllProjects { get; set; } = new();
    }
}
