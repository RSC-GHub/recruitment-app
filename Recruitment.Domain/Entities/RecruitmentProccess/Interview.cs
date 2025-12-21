using Recruitment.Domain.Entities.Recruitment_Proccess;
using Recruitment.Domain.Enums;

namespace Recruitment.Domain.Entities.RecruitmentProccess
{
    public class Interview : BaseEntity
    {
        public int ApplicationId { get; set; }
        public ApplicantApplication Application { get; set; } = null!;

        public int InterviewerId { get; set; }
        public Interviewer Interviewer { get; set; } = null!;
        public DateTime ScheduledDate { get; set; } = DateTime.UtcNow;
        public InterviewType InterviewType { get; set; }
        public InterviewCategory InterviewCategory { get; set; }
        public InterviewStatus InterviewStatus { get; set; } = InterviewStatus.Scheduled;
        public InterviewResult InterviewResult { get; set; } = InterviewResult.Pending;
        public string? Feedback { get; set; }

        public string? InterViewNote { get; set; }
        public int DurationMinutes { get; set; }

        public ICollection<InterviewRejectionReason> RejectionReasons { get; set; } = new List<InterviewRejectionReason>();

    }
}

