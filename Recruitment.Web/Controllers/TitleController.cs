using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.CoreBusiness.Title;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Web.ViewModels.CoreBusiness.Department;
using Recruitment.Web.ViewModels.CoreBusiness.Title;

namespace Recruitment.Web.Controllers
{
    public class TitleController : Controller
    {
        private readonly ITitleService _titleService;
        private readonly IDepartmentService _departmentService;

        public TitleController(ITitleService titleService, IDepartmentService departmentService)
        {
            _titleService = titleService;
            _departmentService = departmentService;
        }

        // GET: Title/Index
        public async Task<IActionResult> Index(
            int page = 1,
            int pageSize = 10,
            string? search = null,
            int? departmentId = null)
        {
            ViewData["ParentController"] = "AppSetup";
            ViewData["ParentTitle"] = "Setup";

            var pagedResult = await _titleService
                .GetPagedAsync(page, pageSize, search, departmentId);

            var viewModel = new TitlesPagedVM
            {
                Items = pagedResult.Items.Select(t => new TitleListViewModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    Departments = t.Departments
                        .Select(d => new DepartmentViewModel
                        {
                            Id = d.Id,
                            Name = d.Name
                        }).ToList()
                }).ToList(),

                Page = pagedResult.Page,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount,
                Search = search,
                SelectedDepartmentId = departmentId
            };

            // Load departments for filter dropdown
            var departments = await _departmentService.GetAllAsync();
            ViewBag.Departments = departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name,
                Selected = departmentId.HasValue && departmentId.Value == d.Id
            }).ToList();

            return View(viewModel);
        }

        // POST: Title/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name, List<int> departmentIds)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    TempData["ErrorMessage"] = "Title name is required";
                    return RedirectToAction(nameof(Index));
                }

                if (departmentIds == null || !departmentIds.Any())
                {
                    TempData["ErrorMessage"] = "At least one department is required";
                    return RedirectToAction(nameof(Index));
                }

                var dto = new CreateTitleDto
                {
                    Name = name,
                    DepartmentIds = departmentIds
                };

                await _titleService.AddAsync(dto);
                TempData["SuccessMessage"] = "Title created successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error creating title: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Title/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string name, List<int> selectedDepartmentIds)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    TempData["ErrorMessage"] = "Title name is required";
                    return RedirectToAction(nameof(Index));
                }

                if (selectedDepartmentIds == null || !selectedDepartmentIds.Any())
                {
                    TempData["ErrorMessage"] = "At least one department is required";
                    return RedirectToAction(nameof(Index));
                }

                var dto = new UpdateTitleDto
                {
                    Id = id,
                    Name = name,
                    DepartmentIds = selectedDepartmentIds
                };

                await _titleService.UpdateAsync(dto);
                TempData["SuccessMessage"] = "Title updated successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating title: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Title/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _titleService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Title deleted successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting title: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}