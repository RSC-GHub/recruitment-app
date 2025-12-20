using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Domain.Entities.RecruitmentProccess
{
    public class Interviewer : BaseEntity
    {
        public string Name { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public ICollection<Interview>? Interviews { get; set; }
    }
}
