using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.RecruitmentProccess.Interview;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;
using Recruitment.Domain.Enums;
using Recruitment.Web.ViewModels.RecruitmentProcess.Interview;
using Recruitment.Web.ViewModels.RecruitmentProcess.Interview.Recruitment.Web.ViewModels.RecruitmentProcess.Interview;

namespace Recruitment.Web.Controllers
{
    public class InterviewController : Controller
    {
        private readonly IInterviewService _interviewService;
        private readonly IApplicantApplicationService _applicantApplicationService;

        public InterviewController(IInterviewService interviewService, IApplicantApplicationService applicantApplicationService)
        {
            _interviewService = interviewService;
            _applicantApplicationService = applicantApplicationService;
        }

        // GET: Interview
        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            var pagedResult = await _interviewService.GetPagedAsync(page, pageSize, search);

            var vm = new InterviewIndexVM
            {
                Search = search,
                Page = page,
                PageSize = pageSize,
                TotalCount = pagedResult.TotalCount,
                Interviews = pagedResult.Items
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateInterview(InterviewCreateVM vm) 
        {
            try
            {
                if (vm.ApplicationId == 0)
                    return Json(new { success = false, message = "ApplicationId is required" });

                var dto = new InterviewCreateUpdateDTO
                {
                    ApplicationId = vm.ApplicationId,
                    Interviewer = vm.Interviewer,
                    ScheduledDate = vm.ScheduledDate,
                    InterviewType = vm.InterviewType,
                    DurationMinutes = vm.DurationMinutes,
                    InterViewNote = vm.InterViewNote,
                };

                var result = await _interviewService.CreateAsync(dto);
                if (result)
                {
                    await _applicantApplicationService.UpdateApplicationStatusAsync(vm.ApplicationId, ApplicationStatus.InterviewScheduled);
                    return Json(new { success = true, message = "Interview created successfully" });
                }


                return Json(new { success = false, message = "Failed to create interview" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

    }
}
