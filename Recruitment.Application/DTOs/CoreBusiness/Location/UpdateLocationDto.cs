namespace Recruitment.Application.DTOs.CoreBusiness.Location
{
    public class UpdateLocationDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CountryId { get; set; }
    }
}
