using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.CoreBusiness.Department
{
    public class DepartmentCreateViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
