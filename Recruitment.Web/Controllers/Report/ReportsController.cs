using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs.Reports;
using Recruitment.Application.Interfaces.Services.Reports;
using Recruitment.Web.ViewModels.Report;

namespace Recruitment.Web.Controllers.Report
{
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<IActionResult> Index()
        {
            var reports = await _reportService.GetAllAsync();

            var vm = reports.Select(r => new ReportListViewModel
            {
                Id = r.Id,
                Name = r.Name,
                StoredProcedure = r.StoredProcedure,
                Description = r.Description,
                IsActive = r.IsActive
            });

            return View(vm);
        }

        // CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReportViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data";
                return RedirectToAction(nameof(Index));
            }

            var dto = new CreateReportDto
            {
                Name = vm.Name,
                StoredProcedure = vm.StoredProcedure,
                Description = vm.Description,
                IsActive = vm.IsActive,
                Parameters = vm.Parameters.Select(p => new CreateReportParameterDto
                {
                    Name = p.Name,
                    DisplayName = p.DisplayName,
                    Type = p.Type,
                    IsRequired = p.IsRequired
                }).ToList()
            };

            await _reportService.CreateAsync(dto);

            TempData["SuccessMessage"] = "Report created successfully";
            return RedirectToAction(nameof(Index));
        }

        // EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateReportViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid data";
                return RedirectToAction(nameof(Index));
            }

            var dto = new UpdateReportDto
            {
                Id = vm.Id,
                Name = vm.Name,
                StoredProcedure = vm.StoredProcedure,
                Description = vm.Description,
                IsActive = vm.IsActive
            };

            await _reportService.UpdateAsync(dto);

            TempData["SuccessMessage"] = "Report updated successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var report = await _reportService.GetByIdAsync(id);

            if (report == null)
                return NotFound();

            var vm = new ReportDetailsViewModel
            {
                Id = report.Id,
                Name = report.Name,
                StoredProcedure = report.StoredProcedure,
                Description = report.Description,
                IsActive = report.IsActive,
                Parameters = report.Parameters.Select(p => new ReportParameterDetailsViewModel
                {
                    Name = p.Name,
                    DisplayName = p.DisplayName,
                    Type = p.Type,
                    IsRequired = p.IsRequired
                }).ToList()
            };

            return View(vm);
        }


        // DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _reportService.DeleteAsync(id);

            TempData["SuccessMessage"] = "Report deleted successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
