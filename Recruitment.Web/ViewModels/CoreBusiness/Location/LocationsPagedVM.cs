namespace Recruitment.Web.ViewModels.CoreBusiness.Location
{
    public class LocationsPagedVM
    {
        public List<LocationListViewModel> Items { get; set; } = new();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public string? Search { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
