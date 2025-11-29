using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.CoreBusiness.Vacancy;
using Recruitment.Application.DTOs.RecruitmentProccess;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;
using Recruitment.Application.Interfaces.Services.UserManagement;
using Recruitment.Domain.Enums;
using Recruitment.Web.ViewModels.CoreBusiness.Vacancy;
using Recruitment.Web.ViewModels.RecruitmentProcess;

namespace Recruitment.Web.Controllers
{
    public class VacancyController : Controller
    {
        private readonly IVacancyService _vacancyService;
        private readonly ITitleService _titleService;
        private readonly IProjectService _projectService;
        private readonly IApplicantService _applicantService;
        private readonly IApplicantApplicationService _ApplicationService;
        
        public VacancyController(ITitleService titleService, IVacancyService vacancyService, IProjectService projectService, IApplicantApplicationService applicantApplicationService, IApplicantService applicantService)
        {
            _vacancyService = vacancyService;
            _titleService = titleService;
            _projectService = projectService;
            _ApplicationService = applicantApplicationService;
            _applicantService = applicantService;
        }

        private async Task PopulateDropdowns(VacancyCreateVM vm)
        {
            vm.TitlesDropdown = (await _titleService.GetAllAsync())
                                .Select(t => new SelectListItem(t.Name, t.Id.ToString()))
                                .ToList();

            vm.ProjectsDropdown = (await _projectService.GetAllProjectsAsync())
                                  .Select(p => new SelectListItem(p.ProjectName, p.Id.ToString()))
                                  .ToList();
        }

        private async Task PopulateDropdowns(VacancyEditVM vm)
        {
            vm.TitlesDropdown = (await _titleService.GetAllAsync())
                                .Select(t => new SelectListItem(t.Name, t.Id.ToString()))
                                .ToList();

            vm.ProjectsDropdown = (await _projectService.GetAllProjectsAsync())
                                  .Select(p => new SelectListItem(p.ProjectName, p.Id.ToString()))
                                  .ToList();
        }

        // GET: Vacancy
        public async Task<IActionResult> Index(string? search, int? titleId, int? projectId, VacancyStatus? status, int page = 1, int pageSize = 10)
        {
            var vacancies = await _vacancyService.GetAllVacanciesAsync();

            if (!string.IsNullOrEmpty(search))
                vacancies = await _vacancyService.SearchVacanciesAsync(search);

            if (titleId.HasValue || projectId.HasValue || status.HasValue)
                vacancies = await _vacancyService.FilterVacanciesAsync(titleId, projectId, status);

            ViewBag.Titles = new SelectList(await _titleService.GetAllAsync(), "Id", "Name", titleId);
            ViewBag.Projects = new SelectList(await _projectService.GetAllProjectsAsync(), "Id", "ProjectName", projectId);
            ViewBag.Statuses = new SelectList(Enum.GetValues(typeof(VacancyStatus)), status);

            // Pagination
            var totalCount = vacancies.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var pagedVacancies = vacancies.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var vmList = pagedVacancies.Select(v => new VacancyListVM
            {
                Id = v.Id,
                TitleName = v.TitleName,
                PositionCount = v.PositionCount,
                EmploymentType = v.EmploymentType,
                Status = v.Status,
                Deadline = v.Deadline,
                ProjectNames = v.ProjectNames
            }).ToList();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;

            return View(vmList);
        }


        // GET: Vacancy/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var vacancy = await _vacancyService.GetVacancyByIdAsync(id);
            if (vacancy == null) return NotFound();

            var vm = new VacancyDetailsVM
            {
                Id = vacancy.Id,
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
                ProjectNames = vacancy.ProjectNames,

                // Audit
                CreatedBy = vacancy.CreatedBy,
                CreatedOn = vacancy.CreatedOn,
                ModifiedBy = vacancy.ModifiedBy,
                ModifiedOn = vacancy.ModifiedOn
            };

            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllApplicants()
        {
            var applicants = await _applicantService.GetAllApplicantsAsync();
            var result = applicants.Select(a => new { a.Id, Name = a.FullName, a.Email, a.CurrentJob }).ToList();
            return Json(result);
        }

        // POST: assign applicant
        [HttpPost]
        public async Task<IActionResult> AssignApplicant([FromBody] AssignApplicantVM vm)
        {
            if (vm.SelectedApplicantId == 0)
                return Json(new { success = false, message = "Please select an applicant" });

            await _ApplicationService.AssignApplicantAsync(new ApplicationCreateDto
            {
                ApplicantId = vm.SelectedApplicantId,
                VacancyId = vm.VacancyId,
                Note = vm.Note
            });

            return Json(new { success = true, message = "Applicant assigned successfully" });
        }


        // GET: Vacancy/Create
        public async Task<IActionResult> Create()
        {
            var vm = new VacancyCreateVM();
            await PopulateDropdowns(vm);
            return View(vm);
        }

        // POST: Vacancy/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VacancyCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(vm);
                return View(vm);
            }

            var dto = new VacancyCreateDTO
            {
                TitleId = vm.TitleId,
                JobDescription = vm.JobDescription,
                Requirements = vm.Requirements,
                Responsibilities = vm.Responsibilities,
                Benefits = vm.Benefits!,
                PositionCount = vm.PositionCount,
                EmploymentType = vm.EmploymentType,
                SalaryRangeMin = vm.SalaryRangeMin,
                SalaryRangeMax = vm.SalaryRangeMax,
                Deadline = vm.Deadline,
                ProjectIds = vm.ProjectIds
            };

            await _vacancyService.CreateVacancyAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // GET: Vacancy/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var vacancy = await _vacancyService.GetVacancyByIdAsync(id);
            if (vacancy == null) return NotFound();

            var vm = new VacancyEditVM
            {
                Id = vacancy.Id,
                TitleId = (await _titleService.GetAllAsync())
                          .FirstOrDefault(t => t.Name == vacancy.TitleName)?.Id ?? 0,
                JobDescription = vacancy.JobDescription,
                Requirements = vacancy.Requirements,
                Responsibilities = vacancy.Responsibilities,
                Benefits = vacancy.Benefits,
                PositionCount = vacancy.PositionCount,
                EmploymentType = Enum.Parse<EmploymentType>(vacancy.EmploymentType),
                Status = Enum.Parse<VacancyStatus>(vacancy.Status),
                SalaryRangeMin = vacancy.SalaryRangeMin,
                SalaryRangeMax = vacancy.SalaryRangeMax,
                Deadline = vacancy.Deadline,
                ProjectIds = (await _projectService.GetAllProjectsAsync())
                             .Where(p => vacancy.ProjectNames.Contains(p.ProjectName))
                             .Select(p => p.Id).ToList()
            };

            await PopulateDropdowns(vm);
            return View(vm);
        }

        // POST: Vacancy/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VacancyEditVM vm)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdowns(vm);
                return View(vm);
            }

            var dto = new VacancyUpdateDTO
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
                Status = vm.Status,
                Deadline = vm.Deadline,
                ProjectIds = vm.ProjectIds
            };

            await _vacancyService.UpdateVacancyAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        // POST: Vacancy/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _vacancyService.DeleteVacancyAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
