namespace Recruitment.Domain.Entities.UserManagement
{
    public class RolePermission : BaseEntity
    {
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public int PermissionId { get; set; }
        public Permission? Permission { get; set; }

        public int GrantedBy { get; set; } // FK to User
        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
    }
}
