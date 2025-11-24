namespace Recruitment.Web.ViewModels.CoreBusiness.Vacancy
{
    public class AssignProjectsModalVM
    {
        public int VacancyId { get; set; }

        // MultiSelect dropdown
        public List<ProjectSelectItemVM> Projects { get; set; } = new();

        // IDs selected after closing modal
        public List<int> SelectedProjectIds { get; set; } = new();
    }
}
