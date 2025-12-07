using Microsoft.AspNetCore.Mvc.Rendering;

namespace Recruitment.Web.ViewModels.Common
{
    public interface IApplicantDropdowns
    {
        IEnumerable<SelectListItem>? Countries { get; set; }
        IEnumerable<SelectListItem>? Currencies { get; set; }
        IEnumerable<SelectListItem>? MilitaryStatuses { get; set; }
        IEnumerable<SelectListItem>? MaritalStatuses { get; set; }
        IEnumerable<SelectListItem>? EducationDegrees { get; set; }
        IEnumerable<SelectListItem>? GenderType { get; set; }
    }

}
