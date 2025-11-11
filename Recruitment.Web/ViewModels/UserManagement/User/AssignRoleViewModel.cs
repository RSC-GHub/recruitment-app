using Microsoft.AspNetCore.Mvc.Rendering;

namespace Recruitment.Web.ViewModels.UserManagement.User
{
    public class AssignRoleViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? SelectedRole { get; set; }
        public IEnumerable<SelectListItem>? Roles { get; set; }
    }
}
