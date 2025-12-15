using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.CoreBusiness.Title;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Web.Authorization;
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

        // GET: Title
        //[HasPermission("Title", "View")]
        public async Task<IActionResult> Index()
        {
            var titles = await _titleService.GetAllAsync();
            var viewModel = titles.Select(t => new TitleListViewModel
            {
                Id = t.Id,
                Name = t.Name
            });
            return View(viewModel);
        }

        [HttpGet]
        //[HasPermission("Title", "View")]
        public async Task<IActionResult> GetDepartmentsByTitle(int titleId)
        {
            var departments = await _titleService.GetDepartmentsByTitleIdAsync(titleId);
            return Json(departments.Select(d => new { d.Id, d.Name }));
        }

        // GET: Title/Details/5
        //[HasPermission("Title", "View")]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _titleService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            var viewModel = new TitleDetailsViewModel
            {
                Id = dto.Id,
                Name = dto.Name
            };

            return View(viewModel);
        }

        // GET: Title/Create
        //[HasPermission("Title", "Create")]
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.GetAllAsync();
            var viewModel = new TitleCreateViewModel
            {
                Departments = departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[HasPermission("Title", "Create")]
        public async Task<IActionResult> Create(TitleCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.GetAllAsync();
                model.Departments = departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                });
                return View(model);
            }

            var dto = new CreateTitleDto
            {
                Name = model.Name,
                DepartmentIds = model.DepartmentIds
            };

            await _titleService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Title/Edit/5
        //[HasPermission("Title", "Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _titleService.GetByIdWithDepartmentsAsync(id);
            if (dto == null)
                return NotFound();

            var departments = await _departmentService.GetAllAsync();

            var viewModel = new TitleEditViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                SelectedDepartmentIds = dto.DepartmentIds,
                DepartmentList = departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                })
            };

            return View(viewModel);
        }

        // POST: Title/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[HasPermission("Title", "Edit")]
        public async Task<IActionResult> Edit(TitleEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _departmentService.GetAllAsync();
                model.DepartmentList = departments.Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                });
                return View(model);
            }

            var dto = new UpdateTitleDto
            {
                Id = model.Id,
                Name = model.Name,
                DepartmentIds = model.SelectedDepartmentIds
            };

            await _titleService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Title/Delete/5
        //[HasPermission("Title", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _titleService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            var viewModel = new TitleDeleteViewModel
            {
                Id = dto.Id,
                Name = dto.Name
            };

            return View(viewModel);
        }

        // POST: Title/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[HasPermission("Title", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _titleService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
