namespace Recruitment.Domain.Entities.RecruitmentProccess
{
    public class RejectionReason : BaseEntity
    {
        public string Reason { get; set; } = string.Empty;
        public ICollection<InterviewRejectionReason> Interviews { get; set; } = new List<InterviewRejectionReason>();

    }
}
