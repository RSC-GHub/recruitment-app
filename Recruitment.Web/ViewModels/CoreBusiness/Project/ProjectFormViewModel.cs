using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Domain.Enums;

namespace Recruitment.Web.ViewModels.CoreBusiness.Project
{
    public class ProjectFormViewModel
    {
        public int Id { get; set; }
        public string ProjectName { get; set; } = string.Empty;
        public ProjectStatus Status { get; set; }
        public int LocationId { get; set; }

        public IEnumerable<SelectListItem>? Locations { get; set; }
    }
}
