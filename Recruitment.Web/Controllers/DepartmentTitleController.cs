using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs.CoreBusiness.DepartmentTitle;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Web.ViewModels.CoreBusiness.DepartmentTitle;

namespace Recruitment.Web.Controllers
{
    public class DepartmentTitleController : Controller
    {
        private readonly IDepartmentTitleService _departmentTitleService;

        public DepartmentTitleController(IDepartmentTitleService departmentTitleService)
        {
            _departmentTitleService = departmentTitleService;
        }

        // GET: DepartmentTitle
        public async Task<IActionResult> Index()
        {
            var dtos = await _departmentTitleService.GetAllAsync();
            var viewModels = dtos.Select(dt => new DepartmentTitleListViewModel
            {
                Id = dt.Id,
                DepartmentId = dt.DepartmentId,
                TitleId = dt.TitleId
            });
            return View(viewModels);
        }

        // GET: DepartmentTitle/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _departmentTitleService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            var viewModel = new DepartmentTitleDetailsViewModel
            {
                Id = dto.Id,
                DepartmentId = dto.DepartmentId,
                TitleId = dto.TitleId
            };

            return View(viewModel);
        }

        // GET: DepartmentTitle/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DepartmentTitle/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentTitleCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new CreateDepartmentTitleDto
            {
                DepartmentId = model.DepartmentId,
                TitleId = model.TitleId
            };

            await _departmentTitleService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: DepartmentTitle/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _departmentTitleService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            var viewModel = new DepartmentTitleEditViewModel
            {
                Id = dto.Id,
                DepartmentId = dto.DepartmentId,
                TitleId = dto.TitleId
            };

            return View(viewModel);
        }

        // POST: DepartmentTitle/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentTitleEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new UpdateDepartmentTitleDto
            {
                Id = model.Id,
                DepartmentId = model.DepartmentId,
                TitleId = model.TitleId
            };

            await _departmentTitleService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: DepartmentTitle/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _departmentTitleService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            var viewModel = new DepartmentTitleDeleteViewModel
            {
                Id = dto.Id,
                DepartmentId = dto.DepartmentId,
                TitleId = dto.TitleId
            };

            return View(viewModel);
        }

        // POST: DepartmentTitle/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _departmentTitleService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
