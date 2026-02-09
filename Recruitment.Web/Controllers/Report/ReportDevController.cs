using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs.Reports;
using Recruitment.Application.Interfaces.Services.Reports;
using Recruitment.Domain.Enums.Reports;
using Recruitment.Web.ViewModels.Report;
using System.Text.Json;

namespace Recruitment.Web.Controllers.Report
{
    public class ReportDevController : Controller
    {
        private readonly IReportService _reportService;

        public ReportDevController(IReportService reportService)
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
                IsActive = r.IsActive,
                Parameters = r.Parameters.Select(p => new ReportParameterDetailsViewModel
                {
                    Name = p.Name,
                    DisplayName = p.DisplayName,
                    Type = p.Type,
                    IsRequired = p.IsRequired
                }).ToList()
            }).ToList();

            var paramTypes = Enum.GetValues(typeof(ReportParameterType)).Cast<ReportParameterType>().Select(e => new
            {
                Id = (int)e,
                Name = e.ToString()
            }).ToList(); ViewBag.ReportParameterTypes = JsonSerializer.Serialize(paramTypes);

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateReportViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            await _reportService.CreateAsync(new CreateReportDto
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
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateReportViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            await _reportService.UpdateAsync(new UpdateReportDto
            {
                Id = vm.Id,
                Name = vm.Name,
                StoredProcedure = vm.StoredProcedure,
                Description = vm.Description,
                IsActive = vm.IsActive,
                Parameters = vm.Parameters.Select(p => new ReportParameterDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    DisplayName = p.DisplayName,
                    Type = Enum.Parse<ReportParameterType>(p.Type),
                    IsRequired = p.IsRequired
                }).ToList()
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _reportService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var report = await _reportService.GetByIdAsync(id);
            if (report == null) return NotFound();
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
    }

}
