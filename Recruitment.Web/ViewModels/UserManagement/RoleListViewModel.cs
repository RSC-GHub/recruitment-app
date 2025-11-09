using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.UserManagement
{
    public class RoleListViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public List<PermissionItemViewModel>? Permissions { get; set; }
    }
}
