namespace Recruitment.Domain.Entities.CoreBusiness
{
    public class Location : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public int CountryId { get; set; }
        public Country? Country { get; set; }

        public ICollection<Project>? Projects { get; set; }
    }
}
