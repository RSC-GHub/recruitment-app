using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Common;
using Recruitment.Application.Interfaces.Persistence.UserManagement;
using Recruitment.Domain.Entities.UserManagement;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories.UserManagement
{
    public class ApplicantRepository : GenericRepository<Applicant>, IApplicantRepository
    {
        public ApplicantRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor) { }
        public async Task<Applicant?> GetApplicantProfileAsync(int applicantId)
        {
            return await _context.Applicants
                .Include(a => a.Country)
                .Include(a => a.Currency)
                .FirstOrDefaultAsync(a => a.Id == applicantId);
        }

        public async Task<PagedResult<Applicant>> GetPagedApplicantsAsync(
            int page,
            int pageSize,
            string? search = null
        )
        {
            var query = _context.Applicants
                .Include(a => a.Country)
                .Include(a => a.Currency)
                .AsQueryable();

            // search support
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(a =>
                    a.FullName.Contains(search) ||
                    a.Email.Contains(search) ||
                    a.City.Contains(search) ||
                    a.Nationality.Contains(search) ||
                    a.PhoneNumber.Contains(search)
                );
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(a => a.FullName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Applicant>(items, totalCount, page, pageSize);
        }
    }
}
