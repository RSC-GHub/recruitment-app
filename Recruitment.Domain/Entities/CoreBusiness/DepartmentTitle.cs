namespace Recruitment.Domain.Entities.CoreBusiness
{
    public class DepartmentTitle : BaseEntity
    {
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public int TitleId { get; set; }
        public Title? Title { get; set; }
    }
}
