using Recruitment.Application.DTOs.CoreBusiness.Country;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Interfaces.Services.CoreBusiness
{
    public interface ICountryService
    {
        Task<IEnumerable<CountryDto>> GetAllAsync();
        Task<CountryDto?> GetByIdAsync(int id);
        Task AddAsync(CreateCountryDto dto);
        Task UpdateAsync(UpdateCountryDto dto);
        Task DeleteAsync(int id);
    }
}
