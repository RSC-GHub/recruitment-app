using System.ComponentModel.DataAnnotations;

namespace Recruitment.Application.DTOs.CoreBusiness.Currency
{
    public class CreateCurrencyDto
    {
        [Required]
        public string Name { get; set; }
    }
}
 