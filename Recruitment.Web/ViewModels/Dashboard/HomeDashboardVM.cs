using Recruitment.Application.DTOs.Audit;

namespace Recruitment.Web.ViewModels.Dashboard
{
    public class HomeDashboardVM
    {
        public int OpenVacanciesCount { get; set; }
        public int TotalApplicationsCount { get; set; }
        public int UnderReviewApplicationsCount { get; set; }
        public int TodaysInterviewsCount { get; set; }

        public int OnHoldApplications { get; set; }
        public int PendingInterviewResults { get; set; }

        public List<RecentActivityDto> RecentActivities { get; set; }
        = new();
    }

}
