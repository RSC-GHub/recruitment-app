namespace Recruitment.Web.ViewModels.CoreBusiness.Country
{
    public abstract class BaseCountryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
