using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.DTOs.Reports;
using Recruitment.Application.Interfaces.Services.Reports;
using Recruitment.Application.Services.Reports;
using Recruitment.Domain.Enums;
using Recruitment.Domain.Enums.Reports;
using Recruitment.Web.ViewModels.Report;
using System.Net;
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

        [HttpGet]
        public async Task<IActionResult> ExportToExcel(int reportId)
        {

            var report = await _reportService.GetByIdAsync(reportId);
            if (report == null)
                return NotFound();
            var data = await _reportExecutionService.ExecuteAsync(reportId, new());

            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var sheet = workbook.Worksheets.Add(report.Name);

            // Header
            for (int col = 0; col < data.Columns.Count; col++)
            {
                sheet.Cell(1, col + 1).Value = data.Columns[col].ColumnName;
                sheet.Cell(1, col + 1).Style.Font.Bold = true;
            }

            // Rows
            for (int row = 0; row < data.Rows.Count; row++)
            {
                for (int col = 0; col < data.Columns.Count; col++)
                {
                    var value = data.Rows[row][col];
                    var cellValue = value == DBNull.Value ? string.Empty : value.ToString();

                    if (!string.IsNullOrWhiteSpace(cellValue) && cellValue.Contains("<"))
                    {
                        cellValue = StripHtml(cellValue);
                    }

                    sheet.Cell(row + 2, col + 1).Value = cellValue;

                }
            }

            sheet.Columns().AdjustToContents();

            // 4️⃣ Return File
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{report.Name}_{DateTime.Now:yyyyMMddHHmm}.xlsx"
            );
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExportToExcel(int reportId, Dictionary<string, string>? parameters)
        {
            var execParams = parameters?
                .ToDictionary(p => p.Key, p => (object?)p.Value)
                ?? new Dictionary<string, object?>();

            var data = await _reportExecutionService.ExecuteAsync(reportId, execParams);
            var report = await _reportService.GetByIdAsync(reportId);

            using var workbook = new ClosedXML.Excel.XLWorkbook();
            var sheet = workbook.Worksheets.Add("Report");

            // Header
            for (int col = 0; col < data.Columns.Count; col++)
            {
                sheet.Cell(1, col + 1).Value = data.Columns[col].ColumnName;
                sheet.Cell(1, col + 1).Style.Font.Bold = true;
            }

            // Rows
            for (int row = 0; row < data.Rows.Count; row++)
            {
                for (int col = 0; col < data.Columns.Count; col++)
                {
                    var value = data.Rows[row][col];

                    var cellValue = value == DBNull.Value ? string.Empty : value.ToString();

                    if (!string.IsNullOrWhiteSpace(cellValue) && cellValue.Contains("<"))
                    {
                        cellValue = StripHtml(cellValue);
                    }

                    sheet.Cell(row + 2, col + 1).Value = cellValue;


                }
            }

            sheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            var fileName = $"{report.Name}_{DateTime.Now:yyyyMMddHHmm}.xlsx";

            return File(
                stream.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileName
            );
        }

        private static string StripHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var text = doc.DocumentNode.InnerText;

            return WebUtility.HtmlDecode(text).Trim();
        }

    }

}
