namespace Recruitment.Web.ViewModels.UserManagement.User
{
    public class UserListItemViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
