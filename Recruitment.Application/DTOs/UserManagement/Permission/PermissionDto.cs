namespace Recruitment.Application.DTOs.UserManagement.Permission
{
    public class PermissionDto
    {
        public int Id { get; set; }
        public string PermissionName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
    }
}
