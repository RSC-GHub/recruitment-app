using System.ComponentModel.DataAnnotations;

namespace Recruitment.Web.ViewModels.CoreBusiness.Currency
{
    public class CreateCurrencyVM
    {
        [Required]
        public string Name { get; set; }
    }
}
