using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.CoreBusiness.Project;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Web.ViewModels.CoreBusiness.Project;
using Recruitment.Web.Authorization;

namespace Recruitment.Web.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ILocationService _locationService;

        public ProjectController(IProjectService projectService, ILocationService locationService)
        {
            _projectService = projectService;
            _locationService = locationService;
        }

        // GET: Project
        //[HasPermission("Project", "View")]
        public async Task<IActionResult> Index()
        {
            // Load all projects
            var projectsDto = await _projectService.GetAllProjectWithLocationAsync();
            var model = projectsDto.Select(p => new ProjectViewModel
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                Status = p.Status,
                LocationId = p.LocationId,
                LocationName = p.LocationName
            }).ToList();

            var locations = await _locationService.GetAllAsync();
            ViewBag.Locations = locations.Select(l => new SelectListItem(l.Name, l.Id.ToString()));

            return View(model);
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
