using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Common;
using Recruitment.Application.DTOs.UserManagement.Applicant;
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
                    a.PhoneNumber.Contains(search) || 
                    a.CurrentJob.Contains(search) ||
                    a.TargetPosition!.Contains(search)
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

        public async Task<List<Applicant>> GetApplicantsWithHistoryAsync(int applicantId)
        {
            var baseApplicant = await _context.Applicants
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == applicantId);

            if (baseApplicant == null)
                return new List<Applicant>();

            var applicants = await _context.Applicants
                .AsNoTracking()
                .Include(a => a.Country)
                .Include(a => a.Currency)
                .Include(a => a.Applications)
                    .ThenInclude(app => app.Vacancy)
                        .ThenInclude(v => v.Title)
                .Include(a => a.Applications)
                    .ThenInclude(app => app.Interviews)
                        .ThenInclude(i => i.Interviewer)
                .Where(a =>
                    a.Email == baseApplicant.Email ||
                    (a.FullName == baseApplicant.FullName && a.PhoneNumber == baseApplicant.PhoneNumber)
                )
                .ToListAsync();

            var reviewerIds = applicants
                .SelectMany(a => a.Applications)
                .Where(app => app.ReviewedBy != null)
                .Select(app => app.ReviewedBy!.Value)
                .Distinct()
                .ToList();

            if (reviewerIds.Any())
            {
                var reviewers = await _context.Users
                    .IgnoreQueryFilters()
                    .Where(u => reviewerIds.Contains(u.Id))
                    .ToDictionaryAsync(u => u.Id);

                foreach (var application in applicants.SelectMany(a => a.Applications))
                {
                    if (application.ReviewedBy != null &&
                        reviewers.TryGetValue(application.ReviewedBy.Value, out var reviewer))
                    {
                        application.Reviewer = reviewer;
                    }
                }
            }

            return applicants;
        }

    }
}
