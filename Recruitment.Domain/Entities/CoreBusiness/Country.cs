using Recruitment.Application.Interfaces.Common;

namespace Recruitment.Domain.Entities.CoreBusiness
{
    public class Country : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        public ICollection<Location>? Locations { get; set; }
    }
}
