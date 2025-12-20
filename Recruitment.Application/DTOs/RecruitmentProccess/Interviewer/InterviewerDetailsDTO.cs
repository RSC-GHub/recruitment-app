namespace Recruitment.Application.DTOs.RecruitmentProccess.Interviewer
{
    public class InterviewerDetailsDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
    }
}
