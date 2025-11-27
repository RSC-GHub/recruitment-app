using Recruitment.Application.DTOs.CoreBusiness.Currency;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Services.CoreBusiness
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CurrencyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(CreateCurrencyDto dto)
        {
            var currency = new Currency
            {
                Name = dto.Name,
            };
            await _unitOfWork.Currencies.AddAsync(currency);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var currency = await _unitOfWork.Currencies.GetByIdAsync(id);
            if (currency == null)
                throw new Exception("Currency not found");
            _unitOfWork.Currencies.Delete(currency);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<CurrencyDto>> GetAllAsync()
        {
            var currencies = await _unitOfWork.Currencies.GetAllAsync();

            return currencies.Select(c => new CurrencyDto
            {
                Id = c.Id,
                Name = c.Name,
            });
        }

        public async Task<CurrencyDto?> GetByIdAsync(int id)
        {
            var currency = await _unitOfWork.Currencies.GetByIdAsync(id);
            if (currency == null)
            {
                return null;
            }
            return new CurrencyDto
            {
                Id = currency.Id,
                Name = currency.Name,
            };
        }

        public async Task UpdateAsync(CurrencyDto dto)
        {
            var currency = await _unitOfWork.Currencies.GetByIdAsync(dto.Id);
            if (currency == null)
                throw new Exception("Currency not found");
            
            currency.Name = dto.Name;

            _unitOfWork.Currencies.Update(currency);
            await _unitOfWork.CompleteAsync();
        }
    }
}
