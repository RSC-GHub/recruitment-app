using AutoMapper;
using Recruitment.Application.DTOs.CoreBusiness.Country;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;


namespace Recruitment.Application.Services.CoreBusiness
{
    public class CountryService : ICountryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CountryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CountryDto>> GetAllAsync()
        {
            var countries = await _unitOfWork.Countries.GetAllAsync();
            return _mapper.Map<IEnumerable<CountryDto>>(countries);
        }

        public async Task<CountryDto?> GetByIdAsync(int id)
        {
            var country = await _unitOfWork.Countries.GetByIdAsync(id);
            return _mapper.Map<CountryDto>(country);
        }

        public async Task AddAsync(CreateCountryDto dto)
        {
            var entity = _mapper.Map<Country>(dto);
            await _unitOfWork.Countries.AddAsync(entity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(UpdateCountryDto dto)
        {
            var entity = await _unitOfWork.Countries.GetByIdAsync(dto.Id);
            if (entity == null) return;

            _mapper.Map(dto, entity);
            _unitOfWork.Countries.Update(entity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _unitOfWork.Countries.GetByIdAsync(id);
            if (entity != null)
            {
                _unitOfWork.Countries.Delete(entity); 
                await _unitOfWork.CompleteAsync();
            }
        }

    }
}
