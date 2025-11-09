using Recruitment.Domain.Entities.UserManagement;

namespace Recruitment.Application.Interfaces.Persistence.UserManagement
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllWithPermissionsAsync();
        Task<Role?> GetByIdWithPermissionsAsync(int id);

    }
}
