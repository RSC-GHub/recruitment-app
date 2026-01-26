using Recruitment.Domain.Enums;

namespace Recruitment.Domain.Workflows
{
    public static class ApplicationWorkflow
    {
        private static readonly Dictionary<ApplicationStatus, ApplicationStatus[]> AllowedTransitions =
            new()
            {
                #region Initial Submission

                // Application is newly submitted by the candidate
                [ApplicationStatus.Submitted] = new[]
                {
                    ApplicationStatus.UnderReview, // Start reviewing the application
                    ApplicationStatus.Rejected     // Rejected without review
                },

                #endregion

                #region Review Phase

                // Application is under HR review
                [ApplicationStatus.UnderReview] = new[]
                {
                    ApplicationStatus.Interviewing, // Candidate selected for interview
                    ApplicationStatus.Rejected      // Rejected after review
                },

                // Application is on hold due to business reasons
                // (e.g. budget, approval, hiring freeze)
                [ApplicationStatus.Pending] = new[]
                {
                    ApplicationStatus.Offered,  // Business decision approved
                    ApplicationStatus.Rejected  // Position cancelled
                },

                #endregion

                #region Interview Phase

                // Candidate is currently in interview process
                [ApplicationStatus.Interviewing] = new[]
                {
                    ApplicationStatus.AcceptedInterview, // Passed interview
                    ApplicationStatus.AcceptedHRInterview,   // Passed HR interview
                    ApplicationStatus.AcceptedTechInterview, // Passed technical interview
                    ApplicationStatus.InterviewOnHold,   // Interview postponed or rescheduled
                    ApplicationStatus.Rejected,            // Failed interview
                    ApplicationStatus.Pending               // Awaiting internal decision
                },

                // Interview postponed or cancelled temporarily
                [ApplicationStatus.InterviewOnHold] = new[]
                {
                    ApplicationStatus.Interviewing, // New interview date scheduled
                    ApplicationStatus.Rejected      // Interview cancelled permanently
                },

                // Candidate passed interview successfully
                [ApplicationStatus.AcceptedInterview] = new[]
                {
                    ApplicationStatus.Offered,                    // Offer sent
                    ApplicationStatus.Pending, // Waiting for internal approval
                    ApplicationStatus.Interviewing                // Additional interview round
                },

                // Candidate passed interview successfully
                [ApplicationStatus.AcceptedHRInterview] = new[]
                {
                    ApplicationStatus.Offered,                    // Offer sent
                    ApplicationStatus.Pending, // Waiting for internal approval
                    ApplicationStatus.Interviewing                // Additional interview round
                },

                // Candidate passed interview successfully
                [ApplicationStatus.AcceptedTechInterview] = new[]
                {
                    ApplicationStatus.Offered,                    // Offer sent
                    ApplicationStatus.Pending, // Waiting for internal approval
                    ApplicationStatus.Interviewing                // Additional interview round
                },

                #endregion

                #region Offer Phase

                // Offer has been sent to the candidate
                [ApplicationStatus.Offered] = new[]
                {
                    ApplicationStatus.AcceptedOffer, // Candidate accepted the offer
                    ApplicationStatus.Rejected,       // Candidate rejected the offer
                },

                // Candidate accepted the offer but has not signed yet
                [ApplicationStatus.AcceptedOffer] = new[]
                {
                    ApplicationStatus.SignedContract, // Contract signed
                    ApplicationStatus.Notshow,        // Candidate did not show up
                    ApplicationStatus.Rejected     // Candidate backed out
                },

                #endregion

                #region Employment Phase

                // Contract signed successfully
                [ApplicationStatus.SignedContract] = new[]
                {
                    ApplicationStatus.Hired,   // Candidate joined the company
                    ApplicationStatus.Notshow  // Candidate did not show up
                },

                // Candidate did not show up on first working day (final state)
                [ApplicationStatus.Notshow] = new[]
                {
                    ApplicationStatus.Rejected  // Marked as rejected
                },

                #endregion

                #region Exceptional States

                // Application rejected at any stage
                // Can only return to review if reconsidered manually
                [ApplicationStatus.Rejected] = new[]
                {
                    ApplicationStatus.UnderReview
                },

                #endregion
            };

        public static void ValidateTransition(ApplicationStatus current, ApplicationStatus next)
        {
            if (!AllowedTransitions.TryGetValue(current, out var allowed) ||
                !allowed.Contains(next))
            {
                throw new InvalidOperationException(
                    $"Invalid transition from {current} to {next}");
            }
        }

        public static IEnumerable<ApplicationStatus> GetAllowedTransitions(ApplicationStatus current)
        {
            return AllowedTransitions.TryGetValue(current, out var allowed)
                ? allowed
                : Array.Empty<ApplicationStatus>();
        }
    }
}
