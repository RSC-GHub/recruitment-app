using Recruitment.Application.DTOs.UserManagement.Permission;

namespace Recruitment.Application.DTOs.UserManagement.Role
{
    public class RoleReadDto
    {
        public int Id { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public ICollection<PermissionDto>? Permissions { get; set; }
    }

}
