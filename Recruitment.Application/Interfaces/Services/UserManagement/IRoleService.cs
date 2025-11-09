using Recruitment.Application.DTOs.CoreBusiness.Country;
using Recruitment.Application.DTOs.UserManagement.Role;

namespace Recruitment.Application.Interfaces.Services.UserManagement
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleReadDto>> GetAllAsync();
        Task<RoleReadDto?> GetByIdAsync(int id);
        Task AddAsync(RoleCreateDto dto);
        Task UpdateAsync(RoleUpdateDto dto);
        Task DeleteAsync(int id);

        Task<IEnumerable<RoleReadDto>> GetAllWithPermissionsAsync();
        Task<RoleReadDto?> GetByIdWithPermissionsAsync(int id);
    }
}
