namespace Recruitment.Domain.Entities.UserManagement
{
    public class Permission : BaseEntity
    {
        public string PermissionName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Resource { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;

        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
