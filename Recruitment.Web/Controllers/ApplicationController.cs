using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.RecruitmentProccess.Application;
using Recruitment.Application.Interfaces.Common;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;
using Recruitment.Application.Services.RecruitmentProccess;
using Recruitment.Domain.Enums;
using Recruitment.Web.Authorization;
using Recruitment.Web.ViewModels.RecruitmentProcess.Application;
using Recruitment.Web.ViewModels.RecruitmentProcess.RejectionReason;

namespace Recruitment.Web.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly IApplicantApplicationService _applicationService;
        private readonly IInterviewerService _interviewerService;
        private readonly IExcelExportService _exportService;
        private readonly IRejectionReasonService _rejectionReasonService;

        public ApplicationController(IApplicantApplicationService applicationService, 
            IExcelExportService exportService,
            IInterviewerService interviewerService,
            IRejectionReasonService rejectionReasonService)
        {
            _applicationService = applicationService;
            _exportService = exportService;
            _interviewerService = interviewerService;
            _rejectionReasonService = rejectionReasonService;
        }

        public async Task<IActionResult> Index(
             ApplicationStatus? status,
                string? search,
                int page = 1,
                int pageSize = 10)
        {
            var pagedResult = await _applicationService
                .GetAllApplicationsAsync(page, pageSize, status, search);

            var vm = new ApplicationIndexVM
            {
                Search = search,
                Status = status,
                Page = page,
                PageSize = pageSize,
                TotalCount = pagedResult.TotalCount,
                Applications = pagedResult.Items,

                StatusList = Enum.GetValues(typeof(ApplicationStatus))
                    .Cast<ApplicationStatus>()
                    .Select(s => new SelectListItem
                    {
                        Value = s.ToString(),
                        Text = s.ToString(),
                        Selected = (status == s)
                    })
                    .ToList()
            };

            return View(vm);
        }


        // GET: /Application/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var application = await _applicationService.GetApplicationDetails(id);
            if (application == null)
                return NotFound();

            ViewBag.CanAddInterview = await _applicationService.CanAddInterviewAsync(id);

            // All Rejection Reasons for Applications
            var allReasons = await _rejectionReasonService.GetByTypeAsync(RejectionReasonType.Offer);

            var vm = new ApplicationDetailVM
            {
                Id = application.Id,
                ApplicantId = application.ApplicantId,
                ApplicantName = application.ApplicantName,
                ApplicantEmail = application.ApplicantEmail,
                PhoneNumber = application.PhoneNumber,
                TargetPosition = application.TargetPosition,
                CurrentJob = application.CurrentJob,
                CurrentEmployer = application.CurrentEmployer,
                ExpectedFirstDate = application.ExpectedFirstDate,
                ActualFirstDate = application.ActualFirstDate,
                VacancyId = application.VacancyId,
                VacancyTitle = application.VacancyTitle,
                VacancyDescription = application.VacancyDescription,
                ApplicationStatus = application.ApplicationStatus,
                ApplicationDate = application.ApplicationDate,
                ReviewedBy = application.ReviewedBy,
                ReviewedByUserName = application.ReviewedByUserName,
                ReviewDate = application.ReviewDate,
                Note = application.Note,
                CVFilePath = application.CV,
                HasFirstInterview = await _applicationService.CanMoveToSecondInterviewAsync(application.Id),

                AllRejectionReasons = allReasons.Select(r => new ReasonsListVM
                {
                    Id = r.Id,
                    Reason = r.Reason
                }).ToList(),

                SelectedRejectionReasons = application.RejectionReasonTexts
            };

            var interviewers = await _interviewerService.GetAllAsync();
            vm.Interviewers = interviewers.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.Name
            }).ToList();

            return View(vm);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Application", "ReviewApplication")]
        public async Task<IActionResult> Review([FromBody] ApplicationReviewVM vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int reviewedBy))
            {
                return Json(new { success = false, message = "User not authenticated" });
            }

            try
            {
                var dto = new ApplicationReviewDto
                {
                    ApplicationId = vm.ApplicationId,
                    ReviewedBy = reviewedBy,
                    ApplicationStatus = vm.ApplicationStatus,
                    Note = vm.Note,
                    ExpectedFirstDate = vm.ExpectedFirstDate,
                    ActualFirstDate = vm.ActualFirstDate
                };

                await _applicationService.ReviewApplicationAsync(dto, vm.RejectionReasonIds);

                return Json(new
                {
                    success = true,
                    message = "Application reviewed successfully."
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateActualStartDate([FromBody] UpdateActualStartDateVM vm)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid data" });

            try
            {
                var dto = new UpdateActualStartDateDto
                {
                    ApplicationId = vm.ApplicationId,
                    ActualFirstDate = vm.ActualFirstDate
                };

                await _applicationService.UpdateActualStartDateAsync(dto);

                return Json(new { success = true, message = "Actual start date updated successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _applicationService.DeleteApplicationAsync(id);

            if (!result)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ExportApplicants(
            ApplicationStatus? status,
            string? search)
        {
            var data = await _applicationService
                .ExportApplicantsAsync(status, search);

            var file = _exportService.ExportApplicants(data);

            return File(
                file,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "Applicants.xlsx"
            );
        }

    }
}
