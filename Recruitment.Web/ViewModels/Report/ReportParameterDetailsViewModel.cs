using Recruitment.Domain.Enums.Reports;

namespace Recruitment.Web.ViewModels.Report
{
    public class ReportParameterDetailsViewModel
    {
        public string Name { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public ReportParameterType Type { get; set; }
        public bool IsRequired { get; set; }
    }
}
