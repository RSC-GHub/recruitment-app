using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces.Persistence.UserManagement;
using Recruitment.Domain.Entities.UserManagement;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories.UserManagement
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
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

    }
}
