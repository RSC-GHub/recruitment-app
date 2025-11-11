using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs.CoreBusiness.Country;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Web.ViewModels.CoreBusiness.Country;
using Recruitment.Web.Authorization;

namespace Recruitment.Web.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        // GET: Country
        [HasPermission("Country", "View")]
        public async Task<IActionResult> Index()
        {
            var dtos = await _countryService.GetAllAsync();
            var vmList = dtos.Select(c => new CountryListViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code
            }).ToList();

            return View(vmList);
        }

        // GET: Country/Create
        [HasPermission("Country", "Create")]
        public IActionResult Create()
        {
            return View(new CreateCountryViewModel());
        }

        // POST: Country/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Country", "Create")]
        public async Task<IActionResult> Create(CreateCountryViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = new CreateCountryDto
            {
                Name = vm.Name,
                Code = vm.Code
            };

            await _countryService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Country/Edit/5
        [HasPermission("Country", "Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _countryService.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = new EditCountryViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Code = dto.Code
            };

            return View(vm);
        }

        // POST: Country/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Country", "Edit")]
        public async Task<IActionResult> Edit(EditCountryViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = new UpdateCountryDto
            {
                Id = vm.Id,
                Name = vm.Name,
                Code = vm.Code
            };

            await _countryService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Country/Delete/5
        [HasPermission("Country", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _countryService.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = new CountryDeleteViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Code = dto.Code
            };

            return View(vm);
        }

        // POST: Country/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Country", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _countryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Country/Details/5
        [HasPermission("Country", "View")]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _countryService.GetByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = new CountryDetailsViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                Code = dto.Code
            };

            return View(vm);
        }
    }
}
