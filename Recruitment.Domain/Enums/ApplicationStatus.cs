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
        SecondChoice = 8,
        Offered = 9,
        Rejected = 10,
        UnQualified = 11,
        OverQualified = 12,
        Hired = 13,
        NoAction = 14
    }
}
