using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.CoreBusiness.Department
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
