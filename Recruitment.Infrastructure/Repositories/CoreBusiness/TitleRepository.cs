using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Recruitment.Application.Common;

namespace Recruitment.Infrastructure.Repositories.CoreBusiness
{
    public class TitleRepository : GenericRepository<Title>, ITitleRepository
    {

        public TitleRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        private async Task<PagedResult<Title>> ToPagedResultAsync(
            IQueryable<Title> query,
            int page,
            int pageSize)
        {
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Title>(
                items,
                totalCount,
                page,
                pageSize
            );
        }
        public async Task<PagedResult<Title>> GetPagedAsync(
        int page,
        int pageSize,
        string? search = null,
        int? departmentId = null)
        {
            var query = _context.Titles
                .Include(t => t.DepartmentTitles!)
                    .ThenInclude(dt => dt.Department!)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t => t.Name.Contains(search));
            }


            if (departmentId.HasValue)
            {
                query = query.Where(t =>
                    t.DepartmentTitles!
                     .Any(dt => dt.DepartmentId == departmentId.Value));
            }

            query = query.OrderBy(t => t.CreatedOn);

            return await ToPagedResultAsync(query, page, pageSize);
        }

        public async Task<IEnumerable<Title>> GetAllWithDepartmentsAsync()
        {
            return await _context.Titles
                .Include(t => t.DepartmentTitles!)
                    .ThenInclude(dt => dt.Department!)
                .ToListAsync();
        }

        public async Task<Title> GetByIdWithDepartmentsAsync(int id)
        {
            var entity = await _context.Titles
                .Include(t => t.DepartmentTitles!)
                    .ThenInclude(dt => dt.Department!)
                .FirstOrDefaultAsync(t => t.Id == id);

            return entity ?? throw new KeyNotFoundException($"Title with Id {id} not found");
        }
    }
}
