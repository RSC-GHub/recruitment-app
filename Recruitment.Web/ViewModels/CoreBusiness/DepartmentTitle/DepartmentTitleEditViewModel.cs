using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.CoreBusiness.DepartmentTitle
{
    public class DepartmentTitleEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required]
        [Display(Name = "Title")]
        public int TitleId { get; set; }
    }
}
