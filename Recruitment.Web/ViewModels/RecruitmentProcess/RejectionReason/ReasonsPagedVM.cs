using Microsoft.AspNetCore.Mvc.Rendering;
using Recruitment.Application.Common;

namespace Recruitment.Web.ViewModels.RecruitmentProcess.RejectionReason
{
    public class ReasonsPagedVM : PagedResult<ReasonsListVM>
    {
        public string? Search { get; set; }
        public List<SelectListItem> ReasonTypes { get; set; } = new();

    }

}
