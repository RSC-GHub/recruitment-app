using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.CoreBusiness.Location;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Web.ViewModels.CoreBusiness.Location;
using Recruitment.Web.Authorization;

namespace Recruitment.Web.Controllers
{
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;
        private readonly ICountryService _countryService;

        public LocationController(
            ILocationService locationService,
            ICountryService countryService)
        {
            _locationService = locationService;
            _countryService = countryService;
        }

        // GET: Location
        //[HasPermission("Location", "View")]
        public async Task<IActionResult> Index(
            int page = 1,
            int pageSize = 10,
            string? search = null,
            int? countryId = null) 
        {
            ViewData["ParentController"] = "AppSetup";
            ViewData["ParentTitle"] = "Setup";

            var pagedResult = await _locationService.GetPagedAsync(page, pageSize, search, countryId);

            var viewModel = new LocationsPagedVM
            {
                Items = pagedResult.Items.Select(l => new LocationListViewModel
                {
                    Id = l.Id,
                    Name = l.Name,
                    CountryId = l.CountryId,
                    CountryName = l.CountryName
                }).ToList(),
                Page = pagedResult.Page,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount,
                Search = search
            };

            var countries = await _countryService.GetAllAsync();
            ViewBag.Countries = countries.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = countryId.HasValue && countryId.Value == c.Id
            }).ToList();

            ViewBag.SelectedCountryId = countryId;

            return View(viewModel);
        }



        // GET: Location/Details/5
        //[HasPermission("Location", "View")]
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _locationService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            var viewModel = new LocationListViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                CountryName = dto.CountryName
            };

            return View(viewModel);
        }

        // GET: Location/Create
        //[HasPermission("Location", "Create")]
        public async Task<IActionResult> Create()
        {
            var countries = await _countryService.GetAllAsync();
            ViewBag.Countries = countries.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });
            return View();
        }

        // POST: Location/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[HasPermission("Location", "Create")]
        public async Task<IActionResult> Create(LocationCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var countries = await _countryService.GetAllAsync();
                ViewBag.Countries = countries.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });
                return View(model);
            }

            var dto = new CreateLocationDto
            {
                Name = model.Name,
                CountryId = model.CountryId
            };

            await _locationService.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Location/Edit/5
        //[HasPermission("Location", "Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _locationService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            var viewModel = new LocationEditViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                CountryId = dto.CountryId
            };

            var countries = await _countryService.GetAllAsync();
            ViewBag.Countries = countries.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            });

            return View(viewModel);
        }

        // POST: Location/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[HasPermission("Location", "Edit")]
        public async Task<IActionResult> Edit(LocationEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var countries = await _countryService.GetAllAsync();
                ViewBag.Countries = countries.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                });
                return View(model);
            }

            var dto = new UpdateLocationDto
            {
                Id = model.Id,
                Name = model.Name,
                CountryId = model.CountryId
            };

            await _locationService.UpdateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Location/Delete/5
        //[HasPermission("Location", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _locationService.GetByIdAsync(id);
            if (dto == null)
                return NotFound();

            var viewModel = new LocationListViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                CountryName = dto.CountryName
            };

            return View(viewModel);
        }

        // POST: Location/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[HasPermission("Location", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _locationService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
