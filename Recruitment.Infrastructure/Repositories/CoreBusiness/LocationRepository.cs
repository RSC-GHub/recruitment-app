using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Common;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories.CoreBusiness
{
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        public LocationRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        private async Task<PagedResult<Location>> ToPagedResultAsync(
            IQueryable<Location> query,
            int page,
            int pageSize)
        {
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Location>(
                items,
                totalCount,
                page,
                pageSize
            );
        }
        public async Task<PagedResult<Location>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            int? countryId = null)
        {
            var query = _context.Locations
                                .Include(l => l.Country)
                                .AsQueryable();

            // Search by name
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r => r.Name.Contains(search));
            }

            // Filter by country
            if (countryId.HasValue)
            {
                query = query.Where(r => r.CountryId == countryId.Value);
            }

            query = query.OrderBy(r => r.CreatedOn);

            return await ToPagedResultAsync(query, page, pageSize);
        }

    }
}
