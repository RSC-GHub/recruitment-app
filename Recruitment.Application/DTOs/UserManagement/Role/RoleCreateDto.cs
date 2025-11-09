namespace Recruitment.Application.DTOs.UserManagement.Role
{
    public class RoleCreateDto
    {
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<int>? PermissionIds { get; set; }
    }
}
