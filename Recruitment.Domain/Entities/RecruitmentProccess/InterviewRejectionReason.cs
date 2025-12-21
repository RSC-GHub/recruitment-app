namespace Recruitment.Domain.Entities.RecruitmentProccess
{
    public class InterviewRejectionReason : BaseEntity
    {
        public int InterviewId { get; set; }
        public Interview Interview { get; set; } = null!;

        public int RejectionReasonId { get; set; }
        public RejectionReason RejectionReason { get; set; } = null!;

    }
}
