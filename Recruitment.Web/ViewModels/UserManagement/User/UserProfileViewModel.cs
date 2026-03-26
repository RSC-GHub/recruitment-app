using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Web.ViewModels.UserManagement.User
{
    public class UserProfileViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public IEnumerable<SelectListItem> Departments { get; set; } = new List<SelectListItem>();
        public bool IsActive { get; set; }

        public List<string> Roles { get; set; } = new();
        public List<int> SelectedRoleIds { get; set; } = new();
        public IEnumerable<SelectListItem> AvailableRoles { get; set; } = new List<SelectListItem>();
        // Projects
        public List<int> SelectedProjectIds { get; set; } = new(); 
        public IEnumerable<SelectListItem> AvailableProjects { get; set; } = new List<SelectListItem>();
        public List<Project> Projects { get; set; } = new();

    }
}