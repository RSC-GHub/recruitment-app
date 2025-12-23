using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.RecruitmentProccess.Interview;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;
using Recruitment.Domain.Enums;
using Recruitment.Web.ViewModels.RecruitmentProcess.Interview;
using Recruitment.Web.ViewModels.RecruitmentProcess.Interview.Recruitment.Web.ViewModels.RecruitmentProcess.Interview;
using Recruitment.Web.ViewModels.RecruitmentProcess.RejectionReason;

namespace Recruitment.Web.Controllers
{
    public class InterviewController : Controller
    {
        private readonly IInterviewService _interviewService;
        private readonly IApplicantApplicationService _applicantApplicationService;
        private readonly IInterviewerService _interviewerService;
        private readonly IRejectionReasonService _rejectionReasonService;

        public InterviewController(IInterviewService interviewService,
            IApplicantApplicationService applicantApplicationService,
            IInterviewerService interviewerService,
            IRejectionReasonService rejectionReasonService)
        {
            _interviewService = interviewService;
            _applicantApplicationService = applicantApplicationService;
            _interviewerService = interviewerService;
            _rejectionReasonService = rejectionReasonService;
        }

        // GET: Interview
        public async Task<IActionResult> Index(
            string? search,
            InterviewStatus? status,
            InterviewResult? result,
            InterviewType? type,
            InterviewCategory? category,
            DateTime? fromDate,
            DateTime? toDate,
            int page = 1,
            int pageSize = 10)
        {
            var pagedResult = await _interviewService.SearchAsync(
                search, status, result, type, category, fromDate, toDate, page, pageSize);

            var vm = new InterviewIndexVM
            {
                Search = search,
                Status = status,
                Result = result,
                Type = type,
                InterviewCategory = category,
                FromDate = fromDate,
                ToDate = toDate,
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
                    InterviewerId = vm.InterviewerId,
                    ScheduledDate = vm.ScheduledDate,
                    InterviewType = vm.InterviewType,
                    InterviewCategory = vm.InterviewCategory,
                    DurationMinutes = vm.DurationMinutes,
                    InterViewNote = vm.InterViewNote
                };

                var created = await _interviewService.CreateAsync(dto);

                if (!created)
                    return Json(new { success = false, message = "Failed to create interview" });

                return Json(new
                {
                    success = true,
                    message = "Interview scheduled successfully"
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _interviewService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            var interviewers = await _interviewerService.GetAllAsync();
            ViewBag.Interviewers = interviewers.Select(i => new SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.Name
            }).ToList();

            var allReasons = await _rejectionReasonService.GetByTypeAsync(RejectionReasonType.Interview);
            
            // Map DTO to VM
            var vm = new InterviewDetailVM
            {
                Id = dto.Id,
                ApplicationId = dto.ApplicationId,
                ApplicantName = dto.ApplicantName,
                ApplicantId = dto.ApplicantId,
                ApplicantEmail = dto.ApplicantEmail,
                PhoneNumber = dto.PhoneNumber,
                VacancyTitle = dto.VacancyTitle,
                EmploymentType = dto.EmploymentType,
                VacancyProjects = dto.VacancyProjects,
                InterviewerId = dto.InterviewerId,
                InterviewerName = dto.InterviewerName,
                ScheduledDate = dto.ScheduledDate,
                InterviewType = dto.InterviewType,
                InterviewCategory = dto.InterviewCategory,
                InterviewStatus = dto.InterviewStatus,
                InterviewResult = dto.InterviewResult,
                Feedback = dto.Feedback,
                DurationMinutes = dto.DurationMinutes,
                InterViewNote = dto.InterViewNote,
                CreatedBy = dto.CreatedBy,
                CreatedOn = dto.CreatedOn,
                ModifiedBy = dto.ModifiedBy,
                ModifiedOn = dto.ModifiedOn,

                AllRejectionReasons = allReasons.Select(r => new ReasonsListVM
                {
                    Id = r.Id,
                    Reason = r.Reason
                }).ToList(),

                SelectedRejectionReasons = dto.RejectionReasonTexts
            };

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateInterview(UpdateInterviewDTO dto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Invalid data." });

            bool updated = await _interviewService.UpdateInterviewAsync(dto);

            if (!updated)
                return Json(new { success = false, message = "Failed to update interview." });

            return Json(new { success = true, message = "Interview updated successfully." });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInterviewResult(UpdateInterviewResultDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Json(new { success = false, message = "Invalid input" });

                var applicationId = await _interviewService.UpdateInterviewResultAsync(
                    dto.Id,
                    dto.InterviewResult,
                    dto.Feedback,
                    dto.Note,
                    dto.RejectionReasonIds
                );

                if (applicationId == null)
                    return Json(new { success = false, message = "Failed to update interview result." });

                return Json(new
                {
                    success = true,
                    message = "Interview completed and application updated successfully."
                });
            }
            catch (Exception ex)
            {
                // Log the exception here
                return Json(new { success = false, message = $"Error: {ex.Message}" });
            }
        }

    }
}
