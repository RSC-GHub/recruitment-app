using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.Report
{
    public class UpdateReportViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string StoredProcedure { get; set; } = null!;

        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }

}
