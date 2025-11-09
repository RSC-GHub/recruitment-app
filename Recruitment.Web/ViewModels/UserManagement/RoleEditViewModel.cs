using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.UserManagement
{
    public class RoleEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Permissions")]
        public List<int>? SelectedPermissionIds { get; set; }

        public List<PermissionItemViewModel>? AllPermissions { get; set; }
    }
}
