using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs.CoreBusiness.Title;
using Recruitment.Application.DTOs.CoreBusiness.Vacancy;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Web.ViewModels.CoreBusiness.Vacancy;

namespace Recruitment.Web.Controllers
{
    public class VacancyController : Controller
    {
        private readonly IVacancyService _vacancyService;
        private readonly ITitleService _titleService;
        private readonly IProjectService _projectService;

        public VacancyController(
            IVacancyService vacancyService,
            ITitleService titleService,
            IProjectService projectService)
        {
            _vacancyService = vacancyService;
            _titleService = titleService;
            _projectService = projectService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var paged = await _vacancyService.GetPagedAsync(page, pageSize);

            var vm = new VacancyIndexVM
            {
                Vacancies = paged.Items.ToList(),
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = paged.TotalCount
            };

            return View(vm);
        }



        // GET: Vacancy/Create
        public async Task<IActionResult> Create()
        {
            var titles = await _titleService.GetAllAsync();

            var vm = new VacancyCreateVM
            {
                Titles = titles.Select(t => new TitleDto
                {
                    Id = t.Id,
                    Name = t.Name
                }).ToList()
            };

            return View(vm);
        }

        // POST: Vacancy/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VacancyCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                var Titles = await _titleService.GetAllAsync();
                vm.Titles.Select(t => new TitleDto
                {
                    Id = t.Id,
                    Name = t.Name
                });
                return View(vm);
            }

            var createdVacancy = await _vacancyService.CreateAsync(vm.Vacancy);

            TempData["Success"] = "Vacancy created successfully.";

            return RedirectToAction("Edit", new { id = createdVacancy.Id });
        }
        // GET: Vacancy/Edit/6
        public async Task<IActionResult> Edit(int id)
        {
            var vacancy = await _vacancyService.GetByIdAsync(id);
            if (vacancy == null)
                return NotFound();

            var titles = await _titleService.GetAllAsync();
            var projects = await _projectService.GetAllProjectsAsync();

            var vm = new VacancyEditSectionVM
            {
                VacancyId = vacancy.Id,
                TitleName = vacancy.TitleName,
                JobDescription = vacancy.JobDescription,
                Requirements = vacancy.Requirements,
                Responsibilities = vacancy.Responsibilities,
                Benefits = vacancy.Benefits,
                PositionCount = vacancy.PositionCount,
                EmploymentType = vacancy.EmploymentType.ToString(),
                SalaryRangeMin = vacancy.SalaryRangeMin,
                SalaryRangeMax = vacancy.SalaryRangeMax,
                Status = vacancy.Status.ToString(),
                Deadline = vacancy.Deadline,
                Titles = titles.Select(t => new TitleDto { Id = t.Id, Name = t.Name }).ToList(),
                AllProjects = projects.Select(p => new ProjectSelectItemVM
                {
                    Id = p.Id,
                    Name = p.ProjectName,
                    Selected = vacancy.Projects.Any(vp => vp.ProjectId == p.Id)
                }).ToList(),
                SelectedProjectIds = vacancy.Projects.Select(p => p.ProjectId).ToList()
            };

            return View(vm); // هيرجع view Edit.cshtml
        }


        [HttpPost]
        public async Task<IActionResult> UpdateBasic(VacancyEditSectionVM vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Please correct the input data.";
                return RedirectToAction("Index", new { editId = vm.VacancyId });
            }

            var dto = new VacancyUpdateDto
            {
                Id = vm.VacancyId,
                TitleId = 0,
                JobDescription = vm.JobDescription,
                Requirements = vm.Requirements,
                Responsibilities = vm.Responsibilities,
                Benefits = vm.Benefits,
                PositionCount = vm.PositionCount,
                EmploymentType = Enum.Parse<Recruitment.Domain.Enums.EmploymentType>(vm.EmploymentType),
                SalaryRangeMin = vm.SalaryRangeMin,
                SalaryRangeMax = vm.SalaryRangeMax,
                Status = Enum.Parse<Recruitment.Domain.Enums.VacancyStatus>(vm.Status),
                Deadline = vm.Deadline
            };

            await _vacancyService.UpdateAsync(dto);

            TempData["Success"] = "Vacancy updated successfully.";

            return RedirectToAction("Index", new { editId = vm.VacancyId });
        }

        public async Task<IActionResult> LoadProjectSelector(int vacancyId)
        {
            var projects = await _projectService.GetAllProjectsAsync();
            var vacancy = await _vacancyService.GetByIdAsync(vacancyId);

            var vm = new AssignProjectsModalVM
            {
                VacancyId = vacancyId,
                Projects = projects.Select(p => new ProjectSelectItemVM
                {
                    Id = p.Id,
                    Name = p.ProjectName,
                    Selected = vacancy?.Projects.Any(vp => vp.ProjectId == p.Id) ?? false
                }).ToList()
            };

            return PartialView("_ProjectSelectorModal", vm);
        }

        [HttpPost]
        public async Task<IActionResult> SaveProjects(AssignProjectsModalVM vm)
        {
            // TODO: بعد إنشاء ProjectVacancyService
            // await _projectVacancyService.UpdateVacancyProjects(vm.VacancyId, vm.Projects.Where(x => x.Selected).Select(x => x.Id).ToList());

            TempData["Success"] = "Projects updated successfully.";

            return RedirectToAction("Index", new { editId = vm.VacancyId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _vacancyService.DeleteAsync(id);
            TempData["Success"] = "Vacancy deleted.";

            return RedirectToAction("Index");
        }
    }
}
