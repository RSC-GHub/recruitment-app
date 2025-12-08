using Recruitment.Domain.Enums;

namespace Recruitment.Domain.Workflows
{
    public static class ApplicationWorkflow
    {
        private static readonly Dictionary<ApplicationStatus, ApplicationStatus[]> AllowedTransitions =
            new()
            {
                [ApplicationStatus.Submitted] = new[]
                {
                    ApplicationStatus.UnderReview,
                    ApplicationStatus.Rejected
                },

                [ApplicationStatus.UnderReview] = new[]
                {
                    ApplicationStatus.Interviewing,
                    ApplicationStatus.OnHold,
                    ApplicationStatus.Rejected
                },

                [ApplicationStatus.Interviewing] = new[]
                {
                    ApplicationStatus.Offered,
                    ApplicationStatus.OnHold,
                    ApplicationStatus.Rejected
                },

                [ApplicationStatus.OnHold] = new[]
                {
                    ApplicationStatus.Interviewing,
                    ApplicationStatus.Rejected
                },

                [ApplicationStatus.Offered] = new[]
                {
                    ApplicationStatus.Hired,
                    ApplicationStatus.Rejected
                },

                [ApplicationStatus.Rejected] = new[]
                {
                    ApplicationStatus.UnderReview
                },
            };

        public static void ValidateTransition(ApplicationStatus current, ApplicationStatus next)
        {
            if (!AllowedTransitions.ContainsKey(current) ||
                !AllowedTransitions[current].Contains(next))
            {
                throw new InvalidOperationException(
                    $"Invalid transition from {current} to {next}");
            }
        }

        // <-- Add this method
        public static IEnumerable<ApplicationStatus> GetAllowedTransitions(ApplicationStatus current)
        {
            if (AllowedTransitions.ContainsKey(current))
                return AllowedTransitions[current];
            return Array.Empty<ApplicationStatus>();
        }
    }
}

