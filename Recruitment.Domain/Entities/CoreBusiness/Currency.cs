using System.ComponentModel.DataAnnotations;

namespace Recruitment.Domain.Entities.CoreBusiness
{
    public class Currency : BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
