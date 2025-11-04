namespace Recruitment.Application.DTOs.CoreBusiness.Location
{
    public class CreateLocationDto
    {
        public string Name { get; set; } = string.Empty;
        public int CountryId { get; set; }
    }
}
