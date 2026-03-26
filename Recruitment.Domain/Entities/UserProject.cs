using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Entities.UserManagement;

namespace Recruitment.Domain.Entities
{
    public class UserProject : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
}
