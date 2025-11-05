using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.CoreBusiness.Title
{
    public class TitleCreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        public List<int> DepartmentIds { get; set; } = new();
        public IEnumerable<SelectListItem>? Departments { get; set; }
    }
}
