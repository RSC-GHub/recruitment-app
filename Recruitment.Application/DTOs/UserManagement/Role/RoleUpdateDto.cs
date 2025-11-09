namespace Recruitment.Application.DTOs.UserManagement.Role
{
    public class RoleUpdateDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        public ICollection<int>? PermissionIds { get; set; }
    }
}
