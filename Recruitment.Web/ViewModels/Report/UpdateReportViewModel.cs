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

        public List<UpdateReportParameterViewModel> Parameters { get; set; } = new List<UpdateReportParameterViewModel>();
    }

    public class UpdateReportParameterViewModel
    {
        public int Id { get; set; } 

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string DisplayName { get; set; } = null!;

        [Required]
        public string Type { get; set; } = "String";

        public bool IsRequired { get; set; }
    }

}
