using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs.CoreBusiness.Department;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Web.ViewModels.CoreBusiness.Department;
using Recruitment.Web.Authorization;

namespace Recruitment.Web.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: Department
        //[HasPermission("Department", "View")]
        public async Task<IActionResult> Index()
        {
            var departments = await _departmentService.GetAllAsync();
            var viewModel = departments.Select(d => new DepartmentViewModel
            {
                Id = d.Id,
                Name = d.Name
            });
            return View(viewModel);
        }

        // GET: Department/Details/5
        //[HasPermission("Department", "View")]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _departmentService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            var viewModel = new DepartmentViewModel
            {
                Id = dto.Id,
                Name = dto.Name
            };

            return View(viewModel);
        }

        // GET: Department/Create
        //[HasPermission("Department", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Department/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[HasPermission("Department", "Create")]
        public async Task<IActionResult> Create(DepartmentCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new CreateDepartmentDto
            {
                Name = model.Name
            };

            await _departmentService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Department/Edit/5
        //[HasPermission("Department", "Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _departmentService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            var viewModel = new DepartmentViewModel
            {
                Id = dto.Id,
                Name = dto.Name
            };

            return View(viewModel);
        }

        // POST: Department/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[HasPermission("Department", "Edit")]
        public async Task<IActionResult> Edit(DepartmentViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new UpdateDepartmentDto
            {
                Id = model.Id,
                Name = model.Name
            };

            await _departmentService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Department/Delete/5
        //[HasPermission("Department", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _departmentService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            var viewModel = new DepartmentViewModel
            {
                Id = dto.Id,
                Name = dto.Name
            };

            return View(viewModel);
        }

        // POST: Department/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[HasPermission("Department", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _departmentService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
