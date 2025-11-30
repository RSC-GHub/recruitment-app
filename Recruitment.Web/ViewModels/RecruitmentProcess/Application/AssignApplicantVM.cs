using Microsoft.AspNetCore.Mvc.Rendering;

namespace Recruitment.Web.ViewModels.RecruitmentProcess.Application
{
    public class AssignApplicantVM
    {
        public int VacancyId { get; set; }  
        public string VacancyTitle { get; set; } = null!; 

        public int SelectedApplicantId { get; set; }  
        public List<SelectListItem> ApplicantsDropdown { get; set; } = new List<SelectListItem>();

        public string? Note { get; set; }
    }
}
