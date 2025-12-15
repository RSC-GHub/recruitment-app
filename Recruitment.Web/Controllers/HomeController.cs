using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.Interfaces.Services.Audit;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;
using Recruitment.Domain.Enums;
using Recruitment.Web.Models;
using Recruitment.Web.ViewModels.Dashboard;
using System.Diagnostics;

namespace Recruitment.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVacancyService _vacancyService;
        private readonly IApplicantApplicationService _applicationService;
        private readonly IInterviewService _interviewService;
        private readonly IAuditService _auditService;

        public HomeController(ILogger<HomeController> logger, 
            IVacancyService vacancyService, 
            IApplicantApplicationService applicationService,
            IInterviewService interviewService,
            IAuditService auditService)
        {
            _logger = logger;
            _vacancyService = vacancyService;
            _applicationService = applicationService;
            _interviewService = interviewService;
            _auditService = auditService;
        }

        public async Task<IActionResult> Index()
        {
            var recentActivities = await _auditService.GetRecentActivitiesAsync(5); 

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

                RecentActivities = recentActivities
            };

            return View(vm);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exception = HttpContext.Items["Exception"] as Exception;

            var model = new ErrorViewModel
            {
                Message = "Something went wrong. Please contact support.",
                Details = exception?.Message,
                TraceId = HttpContext.TraceIdentifier
            };

            return View(model);
        }
    }
}
