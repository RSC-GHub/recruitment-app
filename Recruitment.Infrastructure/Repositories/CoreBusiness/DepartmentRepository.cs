using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Common;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories.CoreBusiness
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        public DepartmentRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
           : base(context, httpContextAccessor)
        {
        }

        public async Task<PagedResult<Department>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null)
        {
            var query = _context.Departments.AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d =>
                    d.Name.Contains(search));
            }

            // Order (important)
            query = query.OrderBy(d => d.CreatedOn);

            return await ToPagedResultAsync(query, page, pageSize);
        }

        private async Task<PagedResult<Department>> ToPagedResultAsync(
            IQueryable<Department> query,
            int page,
            int pageSize)
        {
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedResult<Department>(
                items,
                totalCount,
                page,
                pageSize
            );
        }
    }

}
