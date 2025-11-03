using Recruitment.Domain.Enums;

namespace Recruitment.Domain.Entities.CoreBusiness
{
    public class ProjectVacancy : BaseEntity
    {
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        public int VacancyId { get; set; }
        public Vacancy? Vacancy { get; set; }
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
    }
}
