using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.CoreBusiness.Location
{
    public class LocationCreateViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int CountryId { get; set; }
    }
}
