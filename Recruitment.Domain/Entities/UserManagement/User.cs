namespace Recruitment.Domain.Entities.UserManagement
{
    public class User : BaseEntity
    {
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Department { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? LastLogin { get; set; }

        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
