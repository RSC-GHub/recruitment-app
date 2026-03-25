using Recruitment.Application.DTOs.UserManagement.Permission;

namespace Recruitment.Application.Interfaces.Services.UserManagement
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionDto>> GetAllAsync();
        Task<PermissionDto?> GetByIdAsync(int id);
        Task AddAsync(CreatePermissionDto dto);
        Task UpdateAsync(UpdatePermissionDto dto);
        Task DeleteAsync(int id);

        bool HasPermission(string module, string action);

    }
}
