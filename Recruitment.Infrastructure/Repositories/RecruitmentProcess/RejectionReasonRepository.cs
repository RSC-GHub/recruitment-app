using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Common;
using Recruitment.Application.Interfaces.Persistence.RecruitmentProcess;
using Recruitment.Domain.Entities.RecruitmentProccess;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories.RecruitmentProcess
{
    public class RejectionReasonRepository : GenericRepository<RejectionReason>, IRejectionReasonRepository
    {
        public RejectionReasonRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        private async Task<PagedResult<RejectionReason>> ToPagedResultAsync(
            IQueryable<RejectionReason> query,
            int page,
            int pageSize)
        {
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<RejectionReason>(
                items,
                totalCount,
                page,
                pageSize
            );
        }

        public async Task<PagedResult<RejectionReason>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null)
        {
            var query = _context.RejectionReasons
                                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(r =>
                    r.Reason.Contains(search));
            }

            // Sorting from smallest to largest (oldest first)
            query = query.OrderBy(r => r.CreatedOn);

            return await ToPagedResultAsync(query, page, pageSize);
        }

    }
}
