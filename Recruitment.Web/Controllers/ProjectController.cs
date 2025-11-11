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
        [HasPermission("Project", "View")]
        public async Task<IActionResult> Index()
        {
            var projectsDto = await _projectService.GetAllProjectWithLocationAsync();
            var model = projectsDto.Select(p => new ProjectViewModel
            {
                Id = p.Id,
                ProjectName = p.ProjectName,
                Status = p.Status,
                LocationId = p.LocationId,
                LocationName = p.LocationName
            }).ToList();

            return View(model);
        }

        // GET: Project/Details/5
        [HasPermission("Project", "View")]
        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectService.GetProjectWithLocationAsync(id);
            if (project == null) return NotFound();

            var model = new ProjectViewModel
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Status = project.Status,
                LocationId = project.LocationId,
                LocationName = project.LocationName
            };

            return View(model);
        }

        // GET: Project/Create
        [HasPermission("Project", "Create")]
        public async Task<IActionResult> Create()
        {
            var locations = await _locationService.GetAllAsync();
            var model = new ProjectFormViewModel
            {
                Locations = locations.Select(l => new SelectListItem(l.Name, l.Id.ToString()))
            };

            return View(model);
        }

        // POST: Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Project", "Create")]
        public async Task<IActionResult> Create(ProjectFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var locations = await _locationService.GetAllAsync();
                model.Locations = locations.Select(l => new SelectListItem(l.Name, l.Id.ToString()));
                return View(model);
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

        // GET: Project/Edit/5
        [HasPermission("Project", "Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null) return NotFound();

            var locations = await _locationService.GetAllAsync();

            var model = new ProjectFormViewModel
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Status = project.Status,
                LocationId = project.LocationId,
                Locations = locations.Select(l => new SelectListItem(l.Name, l.Id.ToString()))
            };

            return View("Create", model);
        }

        // POST: Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HasPermission("Project", "Edit")]
        public async Task<IActionResult> Edit(ProjectFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var locations = await _locationService.GetAllAsync();
                model.Locations = locations.Select(l => new SelectListItem(l.Name, l.Id.ToString()));
                return View(model);
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

        // GET: Project/Delete/5
        [HasPermission("Project", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _projectService.GetProjectWithLocationAsync(id);
            if (project == null) return NotFound();

            var model = new ProjectViewModel
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Status = project.Status,
                LocationId = project.LocationId,
                LocationName = project.LocationName
            };

            return View(model);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HasPermission("Project", "Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _projectService.DeleteProjectAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
