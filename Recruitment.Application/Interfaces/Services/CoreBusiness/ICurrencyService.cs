using Recruitment.Application.DTOs.CoreBusiness.Currency;

namespace Recruitment.Application.Interfaces.Services.CoreBusiness
{
    public interface ICurrencyService
    {
        Task<IEnumerable<CurrencyDto>> GetAllAsync();
        Task<CurrencyDto?> GetByIdAsync(int id);
        Task AddAsync(CreateCurrencyDto dto);
        Task UpdateAsync(CurrencyDto dto);
        Task DeleteAsync(int id);
    }
}
