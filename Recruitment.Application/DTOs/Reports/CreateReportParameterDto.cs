using Recruitment.Domain.Enums.Reports;

namespace Recruitment.Application.DTOs.Reports
{
    public class CreateReportParameterDto
    {
        public string Name { get; set; } = null!;          
        public string DisplayName { get; set; } = null!;   
        public ReportParameterType Type { get; set; }
        public bool IsRequired { get; set; }
    }

}
