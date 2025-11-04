namespace Recruitment.Application.DTOs.CoreBusiness.Location
{
    public class LocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public string? CountryName { get; set; }
    }
}
