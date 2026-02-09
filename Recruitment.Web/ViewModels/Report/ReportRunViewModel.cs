using Recruitment.Application.DTOs.Reports;
using Recruitment.Domain.Enums.Reports;
using System.Data;

namespace Recruitment.Web.ViewModels.Report
{
    // GET model
    public class ReportRunViewModel
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; } = null!;
        public List<ReportParameterInputViewModel> Parameters { get; set; } = new();
    }

    public class ReportParameterInputViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public ReportParameterType Type { get; set; }
        public bool IsRequired { get; set; }
    }

    // POST model
    public class ReportRunInputModel
    {
        public int ReportId { get; set; }
        public List<ReportParameterValueModel> Parameters { get; set; } = new();
    }

    public class ReportParameterValueModel
    {
        public string Name { get; set; } = null!;
        public string? Value { get; set; }
    }

    // Result model
    public class ReportResultViewModel
    {
        public ReportDto Report { get; set; } = null!;
        public DataTable Data { get; set; } = new();
    }

}
