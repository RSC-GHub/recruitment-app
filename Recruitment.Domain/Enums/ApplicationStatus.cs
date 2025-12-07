namespace Recruitment.Domain.Enums
{
    public enum ApplicationStatus
    {
        Submitted = 1,
        UnderReview = 2,
        ToInterview = 3,
        ToSecondInterview = 4,
        InterviewScheduled = 5,
        InterviewCancelled = 6,
        InterviewRescheduled = 7,
        InterviewPostponed = 8,
        SecondChoice = 9,
        Offered = 10,
        Rejected = 11,
        UnQualified = 12,
        OverQualified = 13,
        Hired = 14,
        NoAction = 15
    }
}
