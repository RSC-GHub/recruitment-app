using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.RecruitmentProccess.RejectionReason;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;
using Recruitment.Domain.Enums;
using Recruitment.Web.Authorization;
using Recruitment.Web.ViewModels.RecruitmentProcess.RejectionReason;

namespace Recruitment.Web.Controllers
{
    public class RejectionReasonsController : Controller
    {
        private readonly IRejectionReasonService _rejectionReasonService;

        public RejectionReasonsController(IRejectionReasonService rejectionReasonService)
        {
            _rejectionReasonService = rejectionReasonService;
        }

        // GET: RejectionReasons
        [HasPermission("RejectionReason", "View")]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string? search = null)
        {
            ViewData["BreadcrumbTitle"] = "Rejection Reasons";
            ViewData["ParentController"] = "AppSetup";
            ViewData["ParentTitle"] = "Setup";

            var pagedDto = await _rejectionReasonService.GetPagedAsync(page, pageSize, search);

            // Map DTO -> ViewModel
            var vm = new ReasonsPagedVM
            {
                Items = pagedDto.Items.Select(r => new ReasonsListVM
                {
                    Id = r.Id,
                    Reason = r.Reason,
                    ReasonType = r.ReasonType
                }).ToList(),
                Page = pagedDto.Page,
                PageSize = pagedDto.PageSize,
                TotalCount = pagedDto.TotalCount,
                Search = search,

                ReasonTypes = Enum.GetValues(typeof(RejectionReasonType))
                .Cast<RejectionReasonType>()
                .Select(s => new SelectListItem
                {
                    Value = ((int)s).ToString(),
                    Text = s.ToString()
                })
                .ToList()
            };

            return View(vm);
        }

        // POST: RejectionReasons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("RejectionReason", "Create")]
        public async Task<IActionResult> Create(CreateRejectionReasonVM vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please enter a valid rejection reason.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var dto = new CreateReasonDto
                {
                    Reason = vm.Reason,
                    ReasonType = vm.ReasonType
                };

                await _rejectionReasonService.AddAsync(dto);
                TempData["Success"] = "Rejection reason added successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error adding rejection reason: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: RejectionReasons/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("RejectionReason", "Edit")]
        public async Task<IActionResult> Edit(int id, CreateRejectionReasonVM vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid rejection reason data.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var dto = new ReasonDto
                {
                    Id = id,
                    Reason = vm.Reason, 
                    ReasonType = vm.ReasonType
                };

                await _rejectionReasonService.UpdateAsync(dto);
                TempData["Success"] = "Rejection reason updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error updating rejection reason: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: RejectionReasons/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("RejectionReason", "Delete")]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            try
            {
                var dto = await _rejectionReasonService.GetByIdAsync(id);
                if (dto == null)
                {
                    TempData["Error"] = "Rejection reason not found.";
                    return RedirectToAction(nameof(Index));
                }

                await _rejectionReasonService.DeleteAsync(id);
                TempData["Success"] = "Rejection reason deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting rejection reason: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
