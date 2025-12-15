using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs.CoreBusiness.Currency;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Web.ViewModels.CoreBusiness.Currency;

namespace Recruitment.Web.Controllers
{
    public class CurrencyController : Controller
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _currencyService.GetAllAsync();
            var vmList = dtos.Select(c => new CurrencyListVM
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
            return View(vmList);
        }

        // GET: Currency/Create
        public IActionResult Create()
        {
            return View(new CreateCurrencyVM());
        }

        // POST: Currency/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCurrencyVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = new CreateCurrencyDto
            {
                Name = vm.Name
            };

            await _currencyService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _currencyService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            var vm = new CreateCurrencyVM
            {
                Name = dto.Name
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateCurrencyVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var dto = new CurrencyDto
            {
                Id = id,
                Name = vm.Name
            };

            await _currencyService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] int id)
        {
            try
            {
                var dto = await _currencyService.GetByIdAsync(id);
                if (dto == null)
                {
                    TempData["Error"] = "Currency not found.";
                    return RedirectToAction(nameof(Index));
                }

                await _currencyService.DeleteAsync(id);
                TempData["Success"] = "Currency deleted successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting currency: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}