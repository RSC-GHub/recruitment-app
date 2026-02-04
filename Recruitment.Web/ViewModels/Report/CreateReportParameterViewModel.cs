using Recruitment.Domain.Enums.Reports;
using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.Report
{
    public class CreateReportParameterViewModel
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string DisplayName { get; set; } = null!;

        [Required]
        public ReportParameterType Type { get; set; }

        public bool IsRequired { get; set; }
    }
}
