using Recruitment.Application.Common;
using Recruitment.Application.DTOs.CoreBusiness.Location;
using Recruitment.Application.DTOs.RecruitmentProccess.RejectionReason;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Services.CoreBusiness
{
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<LocationDto>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            int? countryId = null)
        {
            var pagedResult = await _unitOfWork.LocationRepository
                                                .GetPagedAsync(page, pageSize, search, countryId);

            var dtoItems = pagedResult.Items.Select(r => new LocationDto
            {
                Id = r.Id,
                Name = r.Name,
                CountryId = r.CountryId,
                CountryName = r.Country?.Name
            }).ToList();

            return new PagedResult<LocationDto>(
                dtoItems,
                pagedResult.TotalCount,
                page,
                pageSize
            );
        }


        public async Task<IEnumerable<LocationDto>> GetAllAsync()
        {
            var locations = await _unitOfWork.Locations.GetAllAsync();

            var result = new List<LocationDto>();
            foreach (var loc in locations)
            {
                var country = await _unitOfWork.Countries.GetByIdAsync(loc.CountryId);
                result.Add(new LocationDto
                {
                    Id = loc.Id,
                    Name = loc.Name,
                    CountryId = loc.CountryId,
                    CountryName = country?.Name
                });
            }
            return result;
        }

        public async Task<LocationDto?> GetByIdAsync(int id)
        {
            var location = await _unitOfWork.Locations.GetByIdAsync(id);
            if (location == null)
                return null;

            var country = await _unitOfWork.Countries.GetByIdAsync(location.CountryId);

            return new LocationDto
            {
                Id = location.Id,
                Name = location.Name,
                CountryId = location.CountryId,
                CountryName = country?.Name
            };
        }

        public async Task AddAsync(CreateLocationDto dto)
        {
            var entity = new Location
            {
                Name = dto.Name,
                CountryId = dto.CountryId
            };

            await _unitOfWork.Locations.AddAsync(entity);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateAsync(UpdateLocationDto dto)
        {
            var existing = await _unitOfWork.Locations.GetByIdAsync(dto.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Location with Id {dto.Id} not found.");

            existing.Name = dto.Name;
            existing.CountryId = dto.CountryId;

            _unitOfWork.Locations.Update(existing);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var location = await _unitOfWork.Locations.GetByIdAsync(id);
            if (location == null)
                throw new KeyNotFoundException($"Location with Id {id} not found.");

            _unitOfWork.Locations.Delete(location);
            await _unitOfWork.CompleteAsync();
        }
    }
}
