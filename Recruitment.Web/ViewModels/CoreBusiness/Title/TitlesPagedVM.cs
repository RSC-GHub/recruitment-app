namespace Recruitment.Web.ViewModels.CoreBusiness.Title
{
    public class TitlesPagedVM
    {
        public List<TitleListViewModel> Items { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string? Search { get; set; }
        public int? SelectedDepartmentId { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }


}
