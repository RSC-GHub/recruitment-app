using Microsoft.AspNetCore.Identity;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Entities.Recruitment_Proccess;

namespace Recruitment.Domain.Entities.UserManagement
{
    public class User : IdentityUser<int>
    {
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public string FullName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime? LastLogin { get; set; }

        // Audit fields
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; } = false;
        public ICollection<RolePermission>? RolePermissions { get; set; }

        public ICollection<ApplicantApplication> ReviewedApplications { get; set; } = new List<ApplicantApplication>();
        public ICollection<UserProject> UserProjects { get; set; } = new List<UserProject>();


    }
}
