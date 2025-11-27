using System.ComponentModel.DataAnnotations;

namespace Recruitment.Application.DTOs.CoreBusiness.Currency
{
    public class CurrencyDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
