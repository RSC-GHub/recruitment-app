using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.RecruitmentProccess.Interviewer;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;
using Recruitment.Web.ViewModels.RecruitmentProcess.Interviewer;

namespace Recruitment.Web.Controllers
{
    public class InterviewersController : Controller
    {
        private readonly IInterviewerService _interviewerService;
        private readonly IDepartmentService _departmentService;

        public InterviewersController(IInterviewerService interviewerService, IDepartmentService departmentService)
        {
            _interviewerService = interviewerService;
            _departmentService = departmentService;
        }


        private async Task LoadDepartmentsAsync()
        {
            var departments = await _departmentService.GetAllAsync();

            ViewBag.Departments = departments
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
                .ToList();
        }

        // GET: Interviewers
        public async Task<IActionResult> Index(
            int page = 1,
            int pageSize = 10,
            string? search = null)
        {
            await LoadDepartmentsAsync();

            var pagedDto =
                await _interviewerService.GetPagedAsync(page, pageSize, search);

            var vm = new InterviewersPagedVM
            {
                Items = pagedDto.Items.Select(i => new InterviewerListVM
                {
                    Id = i.Id,
                    Name = i.Name,
                    DepartmentId = i.DepartmentId,
                    DepartmentName = i.DepartmentName
                }).ToList(),

                Page = pagedDto.Page,
                PageSize = pagedDto.PageSize,
                TotalCount = pagedDto.TotalCount,
                Search = search
            };

            return View(vm);
        }


        // POST: Interviewers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInterviewerVM vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartmentsAsync();

                TempData["Error"] = "Please enter valid interviewer data.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var dto = new InterviewerCreateDTO
                {
                    Name = vm.Name,
                    DepartmentId = vm.DepartmentId
                };

                await _interviewerService.AddAsync(dto);
                TempData["Success"] = "Interviewer added successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error adding interviewer: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Interviewers/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateInterviewerVM vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadDepartmentsAsync();

                TempData["Error"] = "Invalid interviewer data.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var dto = new InterviewerUpdateDTO
                {
                    Id = id,
                    Name = vm.Name,
                    DepartmentId = vm.DepartmentId
                };

                await _interviewerService.UpdateAsync(dto);
                TempData["Success"] = "Interviewer updated successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error updating interviewer: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Interviewers/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            try
            {
                var dto = await _interviewerService.GetByIdAsync(id);
                if (dto == null)
                {
                    TempData["Error"] = "Interviewer not found.";
                    return RedirectToAction(nameof(Index));
                }

                await _interviewerService.DeleteAsync(id);
                TempData["Success"] = "Interviewer deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting interviewer: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
