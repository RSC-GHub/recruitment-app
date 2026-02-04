using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces.Persistence.Reports;
using Recruitment.Domain.Entities.Reports;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories.Reports
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        public ReportRepository(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        public async Task<IEnumerable<Report>> GetActiveAsync()
        {
            return await _context.Reports
                .Where(r => r.IsActive && !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetAllWithParametersAsync()
        {
            return await _context.Reports
                .Include(r => r.Parameters)
                .Where(r => !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Report>> GetActiveWithParametersAsync()
        {
            return await _context.Reports
                .Include(r => r.Parameters)
                .Where(r => r.IsActive && !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<Report?> GetByIdWithParametersAsync(int id)
        {
            return await _context.Reports
                .Include(r => r.Parameters)
                .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
        }
    }
}
