using Recruitment.Application.DTOs.CoreBusiness.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recruitment.Application.Interfaces.Services.CoreBusiness
{
    public interface ILocationService
    {
        Task<IEnumerable<LocationDto>> GetAllAsync();
        Task<LocationDto?> GetByIdAsync(int id);
        Task AddAsync(CreateLocationDto dto);
        Task UpdateAsync(UpdateLocationDto dto);
        Task DeleteAsync(int id);
    }

}
