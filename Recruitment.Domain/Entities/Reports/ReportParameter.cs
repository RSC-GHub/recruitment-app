using Recruitment.Domain.Enums.Reports;

namespace Recruitment.Domain.Entities.Reports
{
    public class ReportParameter : BaseEntity
    {
        public int ReportId { get; set; }
        public string Name { get; set; } = null!;     
        public string DisplayName { get; set; } = null!;
        public ReportParameterType Type { get; set; }   
        public bool IsRequired { get; set; }

        public Report Report { get; set; } = null!;
    }
}
