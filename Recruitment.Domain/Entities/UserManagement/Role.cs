using Microsoft.AspNetCore.Identity;
using Recruitment.Application.Interfaces.Common;

namespace Recruitment.Domain.Entities.UserManagement
{
    public class Role : IdentityRole<int>, ISoftDeletable
    {
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        // Audit fields
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; } = false;
        public ICollection<RolePermission>? RolePermissions { get; set; }
    }
}
