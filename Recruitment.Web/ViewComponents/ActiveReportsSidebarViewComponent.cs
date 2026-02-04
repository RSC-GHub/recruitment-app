using Microsoft.AspNetCore.Mvc;
using Recruitment.Application.Interfaces.Services.Reports;

namespace Recruitment.Web.ViewComponents
{
    public class ActiveReportsSidebarViewComponent : ViewComponent
    {
        private readonly IReportService _reportService;

        public ActiveReportsSidebarViewComponent(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var reports = await _reportService.GetActiveReportsAsync();
            return View(reports); 
        }
    }
}
