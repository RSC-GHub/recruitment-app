using Recruitment.Domain.Entities.UserManagement;

namespace Recruitment.Application.Interfaces.Persistence.UserManagement
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllWithPermissionsAsync();
        Task<Role?> GetByIdWithPermissionsAsync(int id);

        Task<Role> GetByIdAsync(int id);
        Task AddAsync(Role role);
        void Update(Role role);
        void Delete(Role role);

    }
}
