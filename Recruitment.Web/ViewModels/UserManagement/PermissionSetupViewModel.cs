namespace Recruitment.Web.ViewModels.UserManagement
{
    public class PermissionSetupViewModel
    {
        public string SelectedResource { get; set; }
        public string SelectedAction { get; set; }
        public List<string> Resources { get; set; } = new();
        public List<string> Actions { get; set; } = new();
        public List<string> SelectedPermissions { get; set; } = new();
    }

}
