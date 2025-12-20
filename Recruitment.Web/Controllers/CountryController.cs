using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs.CoreBusiness.Country;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Web.ViewModels.CoreBusiness.Country;

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
        public async Task<IActionResult> Index()
        {
            var dtos = await _countryService.GetAllAsync();
            var vmList = dtos.Select(c => new CountryViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code
            }).ToList();

            return View(vmList);
        }

        // POST: Country/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name, string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    TempData["ErrorMessage"] = "Country name is required";
                    return RedirectToAction(nameof(Index));
                }

                if (string.IsNullOrWhiteSpace(code))
                {
                    TempData["ErrorMessage"] = "Country code is required";
                    return RedirectToAction(nameof(Index));
                }

                var dto = new CreateCountryDto
                {
                    Name = name,
                    Code = code
                };

                await _countryService.AddAsync(dto);
                TempData["SuccessMessage"] = "Country created successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error creating country: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Country/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string name, string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    TempData["ErrorMessage"] = "Country name is required";
                    return RedirectToAction(nameof(Index));
                }

                if (string.IsNullOrWhiteSpace(code))
                {
                    TempData["ErrorMessage"] = "Country code is required";
                    return RedirectToAction(nameof(Index));
                }

                var dto = new UpdateCountryDto
                {
                    Id = id,
                    Name = name,
                    Code = code
                };

                await _countryService.UpdateAsync(dto);
                TempData["SuccessMessage"] = "Country updated successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error updating country: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Country/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _countryService.DeleteAsync(id);
                TempData["SuccessMessage"] = "Country deleted successfully";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting country: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}