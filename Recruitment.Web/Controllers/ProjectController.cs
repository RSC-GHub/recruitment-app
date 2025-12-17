using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.CoreBusiness.Project;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Application.Services.CoreBusiness;
using Recruitment.Web.Authorization;
using Recruitment.Web.ViewModels.CoreBusiness.Project;

namespace Recruitment.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ILocationService _locationService;
        private readonly ICountryService _countryService;

        public ProjectController(IProjectService projectService, ILocationService locationService, ICountryService countryService)
        {
            _projectService = projectService;
            _locationService = locationService;
            _countryService = countryService;
        }

        // GET: Project
        //[HasPermission("Project", "View")]
        public async Task<IActionResult> Index(
            int page = 1,
            int pageSize = 10,
            string? search = null,
            int? countryId = null)
        {
            // Projects
            var pagedResult = await _projectService
                .GetPagedAsync(page, pageSize, search, countryId);

            var viewModel = new ProjectsPagedVM
            {
                Items = pagedResult.Items.Select(p => new ProjectViewModel
                {
                    Id = p.Id,
                    ProjectName = p.ProjectName,
                    Status = p.Status,
                    LocationId = p.LocationId,
                    LocationName = p.LocationName
                }).ToList(),

                Page = pagedResult.Page,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount,
                Search = search,
                CountryId = countryId
            };

            var countries = await _countryService.GetAllAsync();
            ViewBag.Countries = countries.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name,
                Selected = countryId.HasValue && c.Id == countryId.Value
            }).ToList();

            var locations = await _locationService.GetAllAsync();

            ViewBag.Locations = locations.Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = $"{l.Name} - {l.CountryName}"
            }).ToList();


            return View(viewModel);
        }




        // POST: Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[HasPermission("Project", "Create")]
        public async Task<IActionResult> Create(ProjectFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var locations = await _locationService.GetAllAsync();
                model.Locations = locations.Select(l => new SelectListItem(l.Name, l.Id.ToString()));
                ViewBag.Locations = model.Locations; 
                return View("Index", await GetProjectsViewModel());
            }

            var dto = new ProjectCreateDto
            {
                ProjectName = model.ProjectName,
                Status = model.Status,
                LocationId = model.LocationId
            };

            await _projectService.CreateProjectAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // POST: Project/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[HasPermission("Project", "Edit")]
        public async Task<IActionResult> Edit(ProjectFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var locations = await _locationService.GetAllAsync();
                model.Locations = locations.Select(l => new SelectListItem(l.Name, l.Id.ToString()));
                ViewBag.Locations = model.Locations; // For modal dropdown
                return View("Index", await GetProjectsViewModel());
            }

            var dto = new ProjectUpdateDto
            {
                Id = model.Id,
                ProjectName = model.ProjectName,
                Status = model.Status,
                LocationId = model.LocationId
            };

            await _projectService.UpdateProjectAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // POST: Project/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[HasPermission("Project", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _projectService.DeleteProjectAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Helper method to reload projects for Index view
        private async Task<List<ProjectViewModel>> GetProjectsViewModel()
        {
            var projectsDto = await _projectService.GetAllProjectWithLocationAsync();
            return projectsDto.Select(p => new ProjectViewModel
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                Status = p.Status,
                LocationId = p.LocationId,
                LocationName = p.LocationName
            }).ToList();
        }
    }

}
