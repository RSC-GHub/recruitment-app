using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.CoreBusiness.Vacancy;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
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

        // ============================
        // 1️⃣ LIST (INDEX)
        // ============================
        public async Task<IActionResult> Index()
        {
            var data = await _vacancyService.GetVacanciesForTableAsync();

            var vm = data.Select(v => new VacancyTableViewModel
            {
                Id = v.Id,
                TitleName = v.TitleName,
                PositionCount = v.PositionCount,
                EmploymentType = v.EmploymentType,
                Status = v.Status,
                Projects = v.Projects.Select(p => new ProjectVacancyTableViewModel
                {
                    ProjectName = p.ProjectName,
                    Priority = p.Priority
                }).ToList()
            }).ToList();

            return View(vm);
        }

        // ============================
        // 2️⃣ DETAILS
        // ============================
        public async Task<IActionResult> Details(int id)
        {
            var dto = await _vacancyService.GetVacancyDetailsAsync(id);
            if (dto == null)
                return NotFound();

            var vm = new VacancyDetailsViewModel
            {
                Id = dto.Id,
                TitleId = dto.TitleId,
                TitleName = dto.TitleName,
                JobDescription = dto.JobDescription,
                Requirements = dto.Requirements,
                Responsibilities = dto.Responsibilities,
                Benefits = dto.Benefits,
                PositionCount = dto.PositionCount,
                EmploymentType = dto.EmploymentType,
                SalaryRangeMin = dto.SalaryRangeMin,
                SalaryRangeMax = dto.SalaryRangeMax,
                Status = dto.Status,
                Deadline = dto.Deadline,
                Projects = dto.Projects.Select(p => new ProjectVacancyDetailsViewModel
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    Priority = p.Priority
                }).ToList()
            };

            return View(vm);
        }

        // ============================
        // 3️⃣ CREATE GET
        // ============================
        public async Task<IActionResult> Create()
        {
            var vm = await BuildVacancyFormViewModel(new VacancyFormViewModel());
            return View(vm);
        }

        // ============================
        // 4️⃣ CREATE POST
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VacancyFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm = await BuildVacancyFormViewModel(vm);
                return View(vm);
            }

            var dto = new VacancyCreateDto
            {
                TitleId = vm.TitleId,
                JobDescription = vm.JobDescription,
                Requirements = vm.Requirements,
                Responsibilities = vm.Responsibilities,
                Benefits = vm.Benefits,
                PositionCount = vm.PositionCount,
                EmploymentType = vm.EmploymentType,
                SalaryRangeMin = vm.SalaryRangeMin,
                SalaryRangeMax = vm.SalaryRangeMax,
                Deadline = vm.Deadline,
                Projects = vm.Projects.Select(p => new ProjectVacancyDto
                {
                    ProjectId = p.ProjectId,
                    Priority = p.Priority
                }).ToList()
            };

            var id = await _vacancyService.CreateVacancyAsync(dto);

            TempData["Success"] = "Vacancy created successfully!";
            return RedirectToAction(nameof(Details), new { id });
        }

        // ============================
        // 5️⃣ EDIT GET
        // ============================
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _vacancyService.GetVacancyDetailsAsync(id);
            if (dto == null)
                return NotFound();

            var vm = new VacancyFormViewModel
            {
                Id = dto.Id,
                TitleId = dto.TitleId,
                JobDescription = dto.JobDescription,
                Requirements = dto.Requirements,
                Responsibilities = dto.Responsibilities,
                Benefits = dto.Benefits,
                PositionCount = dto.PositionCount,
                EmploymentType = dto.EmploymentType,
                SalaryRangeMin = dto.SalaryRangeMin,
                SalaryRangeMax = dto.SalaryRangeMax,
                Deadline = dto.Deadline,
                Projects = dto.Projects.Select(p => new ProjectVacancyFormViewModel
                {
                    ProjectId = p.ProjectId,
                    Priority = p.Priority
                }).ToList()
            };

            vm = await BuildVacancyFormViewModel(vm);
            return View(vm);
        }

        // ============================
        // 6️⃣ EDIT POST
        // ============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VacancyFormViewModel vm)
        {
            if (id != vm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                vm = await BuildVacancyFormViewModel(vm);
                return View(vm);
            }

            var dto = new VacancyUpdateDto
            {
                Id = vm.Id,
                TitleId = vm.TitleId,
                JobDescription = vm.JobDescription,
                Requirements = vm.Requirements,
                Responsibilities = vm.Responsibilities,
                Benefits = vm.Benefits,
                PositionCount = vm.PositionCount,
                EmploymentType = vm.EmploymentType,
                SalaryRangeMin = vm.SalaryRangeMin,
                SalaryRangeMax = vm.SalaryRangeMax,
                Deadline = vm.Deadline,
                Projects = vm.Projects.Select(p => new ProjectVacancyDto
                {
                    ProjectId = p.ProjectId,
                    Priority = p.Priority
                }).ToList()
            };

            var updated = await _vacancyService.UpdateVacancyAsync(id, dto);

            if (!updated)
                return NotFound();

            TempData["Success"] = "Vacancy updated successfully!";
            return RedirectToAction(nameof(Details), new { id });
        }

        // ============================
        // 7️⃣ DELETE GET
        // ============================
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _vacancyService.GetVacancyDetailsAsync(id);
            if (dto == null)
                return NotFound();

            var vm = new VacancyDetailsViewModel
            {
                Id = dto.Id,
                TitleId = dto.TitleId,
                TitleName = dto.TitleName,
                JobDescription = dto.JobDescription,
                Requirements = dto.Requirements,
                Responsibilities = dto.Responsibilities,
                Benefits = dto.Benefits,
                PositionCount = dto.PositionCount,
                EmploymentType = dto.EmploymentType,
                SalaryRangeMin = dto.SalaryRangeMin,
                SalaryRangeMax = dto.SalaryRangeMax,
                Status = dto.Status,
                Deadline = dto.Deadline,
                Projects = dto.Projects.Select(p => new ProjectVacancyDetailsViewModel
                {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    Priority = p.Priority
                }).ToList()
            };

            return View(vm);
        }

        // ============================
        // 8️⃣ DELETE POST
        // ============================
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _vacancyService.DeleteVacancyAsync(id);

            if (!deleted)
                return NotFound();

            TempData["Success"] = "Vacancy deleted successfully!";
            return RedirectToAction(nameof(Index));
        }


        // ============================
        // 🔧 Helper: Build Form ViewModel (Titles + Projects)
        // ============================
        private async Task<VacancyFormViewModel> BuildVacancyFormViewModel(VacancyFormViewModel vm)
        {
            var titles = await _titleService.GetAllAsync();
            var projects = await _projectService.GetAllProjectsAsync();

            vm.Titles = titles.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.Name
            }).ToList();

            vm.ProjectList = projects.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.ProjectName
            }).ToList();

            return vm;
        }
    }
}
