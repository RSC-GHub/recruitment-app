using Recruitment.Web.ViewModels.CoreBusiness.Department;

namespace Recruitment.Web.ViewModels.CoreBusiness.Title
{
    public class TitleListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<DepartmentViewModel> Departments { get; set; } = new();

    }
}
