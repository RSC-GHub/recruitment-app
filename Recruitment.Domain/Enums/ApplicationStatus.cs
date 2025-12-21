namespace Recruitment.Domain.Enums
{
    public enum ApplicationStatus
    {
        Submitted,
        UnderReview,
        Interviewing,

        InterviewOnHold,          // Interview postponed / cancelled temporarily
        AcceptedInterview,

        Pending, // Accepted but waiting for internal reason

        Offered,
        AcceptedOffer,
        SignedContract,

        Hired,
        Notshow,
        Rejected
    }
}