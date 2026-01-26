using Recruitment.Application.DTOs.Audit;
using Recruitment.Application.DTOs.CoreBusiness.Vacancy;
using Recruitment.Application.DTOs.RecruitmentProccess.Interview;

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

        public List<InterviewCalendarDto> CalendarInterviews { get; set; } = new();

        public List<VacancyPositionsChartDTO> VacanciesPositionsChart { get; set; }
        = new();

    }

}
