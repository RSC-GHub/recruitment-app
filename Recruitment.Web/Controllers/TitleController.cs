using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs.CoreBusiness.Title;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Web.ViewModels.CoreBusiness.Title;

namespace Recruitment.Web.Controllers
{
    public class TitleController : Controller
    {
        private readonly ITitleService _titleService;

        public TitleController(ITitleService titleService)
        {
            _titleService = titleService;
        }

        // GET: Title
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

        // GET: Title/Details/5
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
        public IActionResult Create()
        {
            return View();
        }

        // POST: Title/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TitleCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new CreateTitleDto
            {
                Name = model.Name
            };

            await _titleService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Title/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _titleService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            var viewModel = new TitleEditViewModel
            {
                Id = dto.Id,
                Name = dto.Name
            };

            return View(viewModel);
        }

        // POST: Title/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TitleEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new UpdateTitleDto
            {
                Id = model.Id,
                Name = model.Name
            };

            await _titleService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Title/Delete/5
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _titleService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
