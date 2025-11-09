using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.UserManagement
{
    public class PermissionItemViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Permission Name")]
        public string PermissionName { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Resource { get; set; }

        public string? Action { get; set; }

        public bool IsSelected { get; set; }
    }
}
