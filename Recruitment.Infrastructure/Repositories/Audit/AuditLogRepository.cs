using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces.Persistence.Audit;
using Recruitment.Domain.Entities.Aduit;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories.Audit
{
    public class AuditLogRepository : GenericRepository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor) { }
        public async Task<List<AuditLog>> GetRecentAsync(int take = 10)
        {
            return await _context.AuditLogs
                .OrderByDescending(a => a.ChangedOn)
                .Take(take)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}
