using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.CoreBusiness.Vacancy;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Domain.Enums;
using Recruitment.Web.ViewModels.CoreBusiness.Vacancy;

namespace Recruitment.Web.Controllers
{
    public class VacancyController : Controller
    {
        private readonly IVacancyService _vacancyService;
        private readonly IProjectService _projectService;
        private readonly ITitleService _titleService;

        public VacancyController(
            IVacancyService vacancyService,
            IProjectService projectService,
            ITitleService titleService)
        {
            _vacancyService = vacancyService;
            _projectService = projectService;
            _titleService = titleService;
        }

        // GET: Vacancy
        public async Task<IActionResult> Index()
        {
            var vacancies = await _vacancyService.GetAllVacanciesWithProjectsAsync();
            var model = vacancies.Select(v => new VacancyViewModel
            {
                Id = v.Id,
                JobDescription = v.JobDescription,
                Requirements = v.Requirements,
                Responsibilities = v.Responsibilities,
                Benefits = v.Benefits,
                PositionCount = v.PositionCount,
                EmploymentType = v.EmploymentType,
                SalaryRangeMin = v.SalaryRangeMin,
                SalaryRangeMax = v.SalaryRangeMax,
                Status = v.Status,
                Deadline = v.Deadline,
                TitleId = v.TitleId,
                TitleName = v.TitleName,
                ProjectIds = v.ProjectIds,
                ProjectNames = v.ProjectNames,
                ProjectPriorities = v.Projects.Select(p => p.Priority).ToList() 
            }).ToList();


            return View(model);
        }

        // GET: Vacancy/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var vacancy = await _vacancyService.GetVacancyWithProjectsAsync(id);
            if (vacancy == null) return NotFound();

            var model = new VacancyViewModel
            {
                Id = vacancy.Id,
                JobDescription = vacancy.JobDescription,
                Requirements = vacancy.Requirements,
                Responsibilities = vacancy.Responsibilities,
                Benefits = vacancy.Benefits,
                PositionCount = vacancy.PositionCount,
                EmploymentType = vacancy.EmploymentType,
                SalaryRangeMin = vacancy.SalaryRangeMin,
                SalaryRangeMax = vacancy.SalaryRangeMax,
                Status = vacancy.Status,
                Deadline = vacancy.Deadline,
                TitleId = vacancy.TitleId,
                TitleName = vacancy.TitleName,
                ProjectIds = vacancy.ProjectIds,
                ProjectNames = vacancy.ProjectNames
            };

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var titles = await _titleService.GetAllAsync();
            var projects = await _projectService.GetAllProjectsAsync();

            var model = new VacancyFormViewModel
            {
                Titles = titles.Select(t => new SelectListItem(t.Name, t.Id.ToString())),
                ProjectsWithPriority = projects.Select(p => new ProjectWithPriority
                {
                    ProjectId = p.Id,
                    ProjectName = p.ProjectName,
                    Priority = PriorityLevel.Medium,
                    IsSelected = false 
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VacancyFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var titles = await _titleService.GetAllAsync();
                var projects = await _projectService.GetAllProjectsAsync();

                model.Titles = titles.Select(t => new SelectListItem(t.Name, t.Id.ToString()));
                model.ProjectsWithPriority = projects.Select(p => new ProjectWithPriority
                {
                    ProjectId = p.Id,
                    ProjectName = p.ProjectName,
                    Priority = PriorityLevel.Medium,
                    IsSelected = false
                }).ToList();

                return View(model);
            }

            var dto = new VacancyCreateDto
            {
                TitleId = model.TitleId,
                JobDescription = model.JobDescription,
                Requirements = model.Requirements,
                Responsibilities = model.Responsibilities,
                Benefits = model.Benefits,
                PositionCount = model.PositionCount,
                EmploymentType = model.EmploymentType,
                SalaryRangeMin = model.SalaryRangeMin,
                SalaryRangeMax = model.SalaryRangeMax,
                Status = model.Status,
                Deadline = model.Deadline,
                Projects = model.ProjectsWithPriority
                            .Where(p => p.IsSelected) 
                            .Select(p => new ProjectPriorityDto
                            {
                                ProjectId = p.ProjectId,
                                Priority = p.Priority
                            }).ToList()
            };

            await _vacancyService.CreateVacancyAsync(dto);
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Edit(int id)
        {
            var vacancy = await _vacancyService.GetVacancyWithProjectsAsync(id);
            if (vacancy == null) return NotFound();

            var titles = await _titleService.GetAllAsync();
            var allProjects = await _projectService.GetAllProjectsAsync();

            var model = new VacancyFormViewModel
            {
                Id = vacancy.Id,
                TitleId = vacancy.TitleId,
                JobDescription = vacancy.JobDescription,
                Requirements = vacancy.Requirements,
                Responsibilities = vacancy.Responsibilities,
                Benefits = vacancy.Benefits,
                PositionCount = vacancy.PositionCount,
                EmploymentType = vacancy.EmploymentType,
                SalaryRangeMin = vacancy.SalaryRangeMin,
                SalaryRangeMax = vacancy.SalaryRangeMax,
                Status = vacancy.Status,
                Deadline = vacancy.Deadline,
                Titles = titles.Select(t => new SelectListItem(t.Name, t.Id.ToString())),
                ProjectsWithPriority = allProjects.Select(p => new ProjectWithPriority
                {
                    ProjectId = p.Id,
                    ProjectName = p.ProjectName,
                    Priority = vacancy.Projects?
                        .FirstOrDefault(vp => vp.ProjectId == p.Id)?.Priority
                        ?? PriorityLevel.Medium
                }).ToList()
            };

            return View("Create", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VacancyFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var titles = await _titleService.GetAllAsync();
                var allProjects = await _projectService.GetAllProjectsAsync();

                model.Titles = titles.Select(t => new SelectListItem(t.Name, t.Id.ToString()));
                model.ProjectsWithPriority = allProjects.Select(p => new ProjectWithPriority
                {
                    ProjectId = p.Id,
                    ProjectName = p.ProjectName,
                    Priority = PriorityLevel.Medium
                }).ToList();

                return View("Create", model);
            }

            var dto = new VacancyUpdateDto
            {
                Id = model.Id,
                TitleId = model.TitleId,
                JobDescription = model.JobDescription,
                Requirements = model.Requirements,
                Responsibilities = model.Responsibilities,
                Benefits = model.Benefits,
                PositionCount = model.PositionCount,
                EmploymentType = model.EmploymentType,
                SalaryRangeMin = model.SalaryRangeMin,
                SalaryRangeMax = model.SalaryRangeMax,
                Status = model.Status,
                Deadline = model.Deadline,
                Projects = model.ProjectsWithPriority.Select(p => new ProjectPriorityDto
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    Priority = p.Priority
                }).ToList()
            };

            var updated = await _vacancyService.UpdateVacancyAsync(dto);
            if (updated == null)
            {
                TempData["Error"] = "Failed to update the vacancy.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Vacancy updated successfully.";
            return RedirectToAction(nameof(Index));
        }


        // GET: Vacancy/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var vacancy = await _vacancyService.GetVacancyWithProjectsAsync(id);
            if (vacancy == null) return NotFound();

            var model = new VacancyViewModel
            {
                Id = vacancy.Id,
                TitleId = vacancy.TitleId,
                TitleName = vacancy.TitleName,
                JobDescription = vacancy.JobDescription,
                Requirements = vacancy.Requirements,
                Responsibilities = vacancy.Responsibilities,
                Benefits = vacancy.Benefits,
                PositionCount = vacancy.PositionCount,
                EmploymentType = vacancy.EmploymentType,
                SalaryRangeMin = vacancy.SalaryRangeMin,
                SalaryRangeMax = vacancy.SalaryRangeMax,
                Status = vacancy.Status,
                Deadline = vacancy.Deadline,
                ProjectIds = vacancy.ProjectIds,
                ProjectNames = vacancy.ProjectNames
            };

            return View(model);
        }

        // POST: Vacancy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _vacancyService.DeleteVacancyAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
