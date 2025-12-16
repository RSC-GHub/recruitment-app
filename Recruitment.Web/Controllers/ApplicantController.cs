using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.DTOs.RecruitmentProccess.Application;
using Recruitment.Application.DTOs.UserManagement.Applicant;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;
using Recruitment.Application.Interfaces.Services.UserManagement;
using Recruitment.Domain.Enums;
using Recruitment.Web.ViewModels.Common;
using Recruitment.Web.ViewModels.RecruitmentProcess.Application;
using Recruitment.Web.ViewModels.UserManagement.Applicant;

namespace Recruitment.Web.Controllers
{
    public class ApplicantController : Controller
    {
        private readonly IApplicantService _applicantService;
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;
        private readonly IApplicantApplicationService _applicationService;
        private readonly IVacancyService _vacancyService;
        private readonly IWebHostEnvironment _env;

        public ApplicantController(
            IApplicantService applicantService,
            ICountryService countryService,
            ICurrencyService currencyService,
            IWebHostEnvironment env,
            IApplicantApplicationService applicantApplicationService,
            IVacancyService vacancyService)
        {
            _applicantService = applicantService;
            _countryService = countryService;
            _currencyService = currencyService;
            _env = env;
            _applicationService = applicantApplicationService;
            _vacancyService = vacancyService;
        }

        public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 10)
        {
            var paged = await _applicantService.GetPagedApplicantsAsync(page, pageSize, search);

            var vm = new ApplicantIndexVM
            {
                Search = search,
                Page = page,
                PageSize = pageSize,
                TotalCount = paged.TotalCount,
                Applicants = paged.Items.Select(a => new ApplicantListVM
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Email = a.Email,
                    PhoneNumber = a.PhoneNumber,
                    CountryName = a.CountryName,
                    EducationDegree = a.EducationDegree,
                    GraduationYear = a.GraduationYear
                })
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVacancies()
        {
            var vacancies = await _vacancyService.GetOpenedVacanciesAsync();
            var result = vacancies.Select(a => new { a.Id, Name = a.TitleName}).ToList();
            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> AssignVacancy([FromBody] AssignVacancyVM vm)
        {
            if (vm.SelectedVacancyId == 0)
                return Json(new { success = false, message = "Please select a vacancy" });

            try
            {
                await _applicationService.AssignApplicantAsync(new ApplicationCreateDto
                {
                    ApplicantId = vm.AppliantId,
                    VacancyId = vm.SelectedVacancyId,
                    Note = vm.Note
                });

                return Json(new { success = true, message = "Applicant assigned successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Server error: " + ex.Message });
            }
        }

        // GET: Applicant/History/5
        public async Task<IActionResult> History(int id)
        {
            var dto = await _applicantService.GetApplicantHistoryAsync(id);
            if (dto == null) return NotFound();

            // Map DTO to VM
            var vm = new ApplicantHistoryVM
            {
                Id = dto.Id,
                FullName = dto.FullName,
                Email = dto.Email!,
                PhoneNumber = dto.PhoneNumber!,
                CountryName = dto.CountryName,
                CityName = dto.CityName,

                Applications = dto.Applications.Select(app => new ApplicationHistoryVM
                {
                    Id = app.Id,
                    VacancyTitle = app.VacancyTitle,
                    ApplicationStatus = app.ApplicationStatus,
                    ApplicationDate = app.ApplicationDate,
                    ReviewedByUserName = app.ReviewedByUserName,
                    Note = app.Note,
                    Interviews = app.Interviews.Select(i => new InterviewHistoryVM
                    {
                        Id = i.Id,
                        InterViewer = i.InterViewer!,
                        ScheduledDate = i.ScheduledDate,
                        InterviewType = i.InterviewType,
                        InterviewCategory = i.InterviewCategory,
                        InterviewStatus = i.InterviewStatus,
                        InterviewResult = i.InterviewResult,
                        Feedback = i.Feedback,
                        InterViewNote = i.InterViewNote,
                    }).ToList()
                }).ToList()
            };

            return View(vm);
        }
        public async Task<IActionResult> Create()
        {
            var vm = new ApplicantCreateVM();
            await LoadDropdowns(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicantCreateVM vm)
        {
            if (vm.Gender == Gender.Female)
            {
                ModelState.Remove(nameof(vm.MilitaryStatus));
                vm.MilitaryStatus = MilitaryStatus.NotApplicable; 
            }

            if (vm.CV != null)
            {
                var allowedExtensions = new[] { ".pdf", ".docx" };
                var fileExtension = Path.GetExtension(vm.CV.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError(nameof(vm.CV), "Only PDF files are allowed.");
                }

                if (vm.CV.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError(nameof(vm.CV), "File size must not exceed 5MB.");
                }
            }
            else
            {
                ModelState.AddModelError(nameof(vm.CV), "CV file is required.");
            }

            if (!ModelState.IsValid)
            {
                await LoadDropdowns(vm);
                return View(vm);
            }

            try
            {
                var dto = new ApplicantCreateDto
                {
                    FullName = vm.FullName,
                    Email = vm.Email,
                    PhoneNumber = vm.PhoneNumber,
                    CountryId = vm.CountryId,
                    City = vm.City,
                    Nationality = vm.Nationality,
                    CurrentJob = vm.CurrentJob,
                    CurrentEmployer = vm.CurrentEmployer,
                    CurrentSalary = vm.CurrentSalary,
                    ExpectedSalary = vm.ExpectedSalary,
                    CurrencyId = vm.CurrencyId,
                    Address = vm.Address,
                    Gender = vm.Gender,
                    MilitaryStatus = vm.MilitaryStatus,
                    MaritalStatus = vm.MaritalStatus,
                    EducationDegree = vm.EducationDegree,
                    GraduationYear = vm.GraduationYear,
                    Major = vm.Major,
                    NoticePeriod = vm.NoticePeriod,
                    ExtraCertificate = vm.ExtraCertificate,
                    CV = vm.CV
                };

                await _applicantService.CreateApplicantAsync(dto);

                // Add success message
                TempData["SuccessMessage"] = "Applicant created successfully!";

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, "An error occurred while creating the applicant. Please try again.");
                await LoadDropdowns(vm);
                return View(vm);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _applicantService.GetApplicantByIdAsync(id);
            if (dto == null) return NotFound();

            var vm = new ApplicantEditVM
            {
                Id = dto.Id,
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                CountryId = dto.CountryId,
                City = dto.City,
                Nationality = dto.Nationality,
                CurrentJob = dto.CurrentJob,
                CurrentEmployer = dto.CurrentEmployer,
                CurrentSalary = dto.CurrentSalary,
                ExpectedSalary = dto.ExpectedSalary,
                CurrencyId = dto.CurrencyId,
                Address = dto.Address,
                Gender = dto.Gender,
                MilitaryStatus = dto.MilitaryStatus,
                MaritalStatus = dto.MaritalStatus,
                EducationDegree = dto.EducationDegree,
                GraduationYear = dto.GraduationYear,
                Major = dto.Major,
                NoticePeriod = dto.NoticePeriod,
                ExtraCertificate = dto.ExtraCertificate,
                ExistingCVPath = dto.CVFilePath,
            };

            await LoadDropdowns(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicantEditVM vm)
        {
            // Conditional validation for MilitaryStatus
            if (vm.Gender == Gender.Female)
            {
                ModelState.Remove(nameof(vm.MilitaryStatus));
                vm.MilitaryStatus = MilitaryStatus.NotApplicable;
            }

            // CV validation if a new file is uploaded
            if (vm.CV != null)
            {
                var allowedExtensions = new[] { ".pdf", ".docx" };
                var fileExtension = Path.GetExtension(vm.CV.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError(nameof(vm.CV), "Only PDF or Word files are allowed.");
                }

                if (vm.CV.Length > 5 * 1024 * 1024)
                {
                    ModelState.AddModelError(nameof(vm.CV), "File size must not exceed 5MB.");
                }
            }

            if (!ModelState.IsValid)
            {
                await LoadDropdowns(vm);
                return View(vm);
            }

            var dto = new ApplicantUpdateDto
            {
                Id = vm.Id,
                FullName = vm.FullName,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber,
                CountryId = vm.CountryId,
                City = vm.City,
                Nationality = vm.Nationality,
                CurrentJob = vm.CurrentJob,
                CurrentEmployer = vm.CurrentEmployer,
                CurrentSalary = vm.CurrentSalary,
                ExpectedSalary = vm.ExpectedSalary,
                CurrencyId = vm.CurrencyId,
                Address = vm.Address,
                Gender = vm.Gender,
                MilitaryStatus = vm.MilitaryStatus,
                MaritalStatus = vm.MaritalStatus,
                EducationDegree = vm.EducationDegree,
                GraduationYear = vm.GraduationYear,
                Major = vm.Major,
                NoticePeriod = vm.NoticePeriod,
                ExtraCertificate = vm.ExtraCertificate,
                CV = vm.CV
            };

            await _applicantService.UpdateApplicantAsync(dto);

            TempData["SuccessMessage"] = "Applicant updated successfully!";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Profile(int id)
        {
            var dto = await _applicantService.GetApplicantProfileAsync(id);
            if (dto == null) return NotFound();

            var vm = new ApplicantProfileVM
            {
                Id = dto.Id,
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,

                CountryId = dto.CountryId,
                CountryName = dto.CountryName,
                City = dto.City,
                Nationality = dto.Nationality,

                CurrentJob = dto.CurrentJob,
                CurrentEmployer = dto.CurrentEmployer,
                CurrentSalary = dto.CurrentSalary,
                ExpectedSalary = dto.ExpectedSalary,

                CurrencyId = dto.CurrencyId,
                CurrencyName = dto.CurrencyName,

                Address = dto.Address,
                Gender = dto.Gender,
                MilitaryStatus = dto.MilitaryStatus,
                MaritalStatus = dto.MaritalStatus,

                EducationDegree = dto.EducationDegree,
                GraduationYear = dto.GraduationYear,
                Major = dto.Major,
                NoticePeriod = dto.NoticePeriod,
                ExtraCertificate = dto.ExtraCertificate,
                CVFilePath = dto.CVFilePath,

                CreatedBy = dto.CreatedBy,
                CreatedOn = dto.CreatedOn,
                ModifiedBy = dto.ModifiedBy,
                ModifiedOn = dto.ModifiedOn
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult DownloadCV(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                return NotFound();

            var fullPath = Path.Combine(_env.WebRootPath, filePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (!System.IO.File.Exists(fullPath))
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            var fileName = Path.GetFileName(fullPath); // you can store original file name if needed

            return File(fileBytes, "application/octet-stream", fileName);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _applicantService.DeleteApplicantAsync(id);
            return RedirectToAction("Index");
        }

        // ---------------------------------------------------------
        private async Task LoadDropdowns(IApplicantDropdowns vm)
        {
            vm.Countries = (await _countryService.GetAllAsync())
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            vm.Currencies = (await _currencyService.GetAllAsync())
                .Select(x => new SelectListItem(x.Name, x.Id.ToString()));

            vm.MilitaryStatuses = Enum.GetValues(typeof(MilitaryStatus))
                .Cast<MilitaryStatus>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                });

            vm.MaritalStatuses = Enum.GetValues(typeof(MaritalStatus))
                .Cast<MaritalStatus>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                });

            vm.EducationDegrees = Enum.GetValues(typeof(EducationDegree))
                .Cast<EducationDegree>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                });

            vm.GenderType = Enum.GetValues(typeof(Gender))
                .Cast<Gender>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                });
        }
    }
}
