using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs.Reports;
using Recruitment.Application.Interfaces.Services.Reports;
using Recruitment.Application.Services.Reports;
using Recruitment.Domain.Enums.Reports;
using Recruitment.Web.ViewModels.Report;
using System.Text.Json;

namespace Recruitment.Web.Controllers.Report
{
    public class ReportsController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IReportExecutionService _reportExecutionService;

        public ReportsController(
            IReportService reportService,
            IReportExecutionService reportExecutionService)
        {
            _reportService = reportService;
            _reportExecutionService = reportExecutionService;
        }

        public async Task<IActionResult> Index()
        {
            var reports = await _reportService.GetAllAsync();
            return View(reports);
        }

        // GET: /Reports/Run/5
        public async Task<IActionResult> Run(int id)
        {
            var report = await _reportService.GetByIdAsync(id);
            if (report == null) return NotFound();

            if (!report.Parameters.Any())
            {
                var data = await _reportExecutionService.ExecuteAsync(id, new());
                return View("ReportResult", new ReportResultViewModel { Report = report, Data = data });
            }

            var vm = new ReportRunViewModel
            {
                ReportId = report.Id,
                ReportName = report.Name,
                Parameters = report.Parameters.Select(p => new ReportParameterInputViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    DisplayName = p.DisplayName,
                    Type = p.Type,
                    IsRequired = p.IsRequired
                }).ToList()
            };

            return View("RunReport", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Run(ReportRunInputModel input)
        {
            var parameters = input.Parameters.ToDictionary(
                p => p.Name,
                p => (object?)p.Value
            );

            var data = await _reportExecutionService.ExecuteAsync(input.ReportId, parameters);
            var report = await _reportService.GetByIdAsync(input.ReportId);

            var runVm = new ReportRunViewModel
            {
                ReportId = report.Id,
                ReportName = report.Name,
                Parameters = report.Parameters.Select(p => new ReportParameterInputViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    DisplayName = p.DisplayName,
                    Type = p.Type,
                    IsRequired = p.IsRequired
                }).ToList()
            };

            ViewBag.ReportResult = new ReportResultViewModel
            {
                Report = report,
                Data = data
            };

            return View("RunReport", runVm);
        }

    }

}
