using Microsoft.AspNetCore.Mvc.Rendering;

namespace Recruitment.Web.ViewModels.RecruitmentProcess.Application
{
    public class AssignVacancyVM
    {
        public int AppliantId { get; set; }
        public string ApplicantName { get; set; } = null!;

        public int SelectedVacancyId { get; set; }
        public List<SelectListItem> VacanciesDropdown { get; set; } = new List<SelectListItem>();

        public string? Note { get; set; }
    }
}
