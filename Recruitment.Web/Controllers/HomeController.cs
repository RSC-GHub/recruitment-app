using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;
using Recruitment.Domain.Enums;
using Recruitment.Web.Models;
using Recruitment.Web.ViewModels.Dashboard;

namespace Recruitment.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVacancyService _vacancyService;
        private readonly IApplicantApplicationService _applicationService;
        private readonly IInterviewService _interviewService;
        //private readonly IAuditService _auditService;

        public HomeController(ILogger<HomeController> logger, 
            IVacancyService vacancyService, 
            IApplicantApplicationService applicationService,
            IInterviewService interviewService
            )
        {
            _logger = logger;
            _vacancyService = vacancyService;
            _applicationService = applicationService;
            _interviewService = interviewService;
            //_auditService = auditService;
        }

        public async Task<IActionResult> Index()
        {
            var calendarInterviews = await _interviewService.GetInterviewsForCalendarAsync();

            var vm = new HomeDashboardVM
            {
                OpenVacanciesCount =
                    await _vacancyService.CountOpenedVacanciesAsync(),

                TotalApplicationsCount =
                    await _applicationService.CountApplicationsAsync(),

                UnderReviewApplicationsCount =
                    await _applicationService.CountApplicationsAsync(ApplicationStatus.UnderReview),

                TodaysInterviewsCount =
                    await _interviewService.CountTodaysInterviewsAsync(),

                OnHoldApplications =
                    await _applicationService.GetOnHoldApplicationsAlertAsync(3),

                PendingInterviewResults =
                    await _interviewService.GetPendingInterviewResultsAlertAsync(),


                CalendarInterviews = calendarInterviews,

                VacanciesPositionsChart =
                    await _vacancyService.GetVacanciesPositionsChartAsync()

            };

            return View(vm);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ErrorModal()
        {
            var model = new ErrorViewModel
            {
                Message = TempData["ErrorMessage"]?.ToString(),
                Details = TempData["ErrorDetails"]?.ToString(),
                TraceId = TempData["TraceId"]?.ToString()
            };

            return View(model);
        }


    }
}
//var recentActivities = await _auditService.GetRecentActivitiesAsync(5);