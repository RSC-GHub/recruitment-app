namespace Recruitment.Web.ViewModels.CoreBusiness.Location
{
    public class LocationListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? CountryName { get; set; }
        public int CountryId { get; set; }
    }
}
