using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces.Persistence.UserManagement;
using Recruitment.Domain.Entities.UserManagement;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories.UserManagement
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllWithPermissionsAsync()
        {
            return await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .ToListAsync();
        }

        public async Task<Role?> GetByIdWithPermissionsAsync(int id)
        {
            return await _context.Roles
                .Include(r => r.RolePermissions)
                    .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddAsync(Role role) => await _context.Roles.AddAsync(role);
        public void Update(Role role) => _context.Roles.Update(role);
        public void Delete(Role role) => _context.Roles.Remove(role);

        public async Task<Role> GetByIdAsync(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(i => i.Id == id);
        }
    }


}
