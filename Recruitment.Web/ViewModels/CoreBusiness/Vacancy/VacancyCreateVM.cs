using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.CoreBusiness.Vacancy
{
    public class VacancyCreateVM
    {
        [Required]
        public int TitleId { get; set; }

        [Required]
        public string JobDescription { get; set; } = string.Empty;

        [Required]
        public string Requirements { get; set; } = string.Empty;

        public string? Responsibilities { get; set; }

        public string? Benefits { get; set; }

        [Required]
        public int PositionCount { get; set; } = 1;

        [Required]
        public EmploymentType EmploymentType { get; set; }

        public decimal? SalaryRangeMin { get; set; }
        public decimal? SalaryRangeMax { get; set; }

        public DateTime? Deadline { get; set; }

        // MultiSelect Projects
        [Display(Name = "Projects")]
        public List<int>? ProjectIds { get; set; } = new();

        public List<SelectListItem>? TitlesDropdown { get; set; }
        public List<SelectListItem>? ProjectsDropdown { get; set; }
    }
}
