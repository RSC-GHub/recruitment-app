using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Common;
using Recruitment.Application.DTOs.UserManagement.Applicant;
using Recruitment.Application.Interfaces.Persistence.UserManagement;
using Recruitment.Domain.Entities.Recruitment_Proccess;
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
            string? search = null)
        {
            var baseQuery = _context.Applicants
                .Include(a => a.Country)
                .Include(a => a.Currency)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var matchedApplicants = await baseQuery
                    .Where(a =>
                        a.FullName.Contains(search) ||
                        a.Email.Contains(search) ||
                        a.City.Contains(search) ||
                        a.Nationality.Contains(search) ||
                        a.PhoneNumber.Contains(search) ||
                        a.CurrentJob.Contains(search) ||
                        a.TargetPosition!.Contains(search)
                    )
                    .Select(a => new
                    {
                        a.Id,
                        MasterId = a.MasterApplicantId ?? a.Id
                    })
                    .ToListAsync();

                var masterIds = matchedApplicants
                    .Select(x => x.MasterId)
                    .Distinct()
                    .ToList();

                baseQuery = baseQuery.Where(a =>
                    masterIds.Contains(a.Id) ||
                    (a.MasterApplicantId != null && masterIds.Contains(a.MasterApplicantId.Value)));
            }

            var totalCount = await baseQuery.CountAsync();

            var items = await baseQuery
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

            var masterId = baseApplicant.MasterApplicantId ?? baseApplicant.Id;

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
                .Where(a => a.Id == masterId || a.MasterApplicantId == masterId)
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

        public async Task<List<ApplicantDuplicateDto>> GetApplicantDuplicatesWithOwnerInfoAsync(int applicantId)
        {
            var applicant = await _context.Applicants
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == applicantId);

            if (applicant == null)
                return new List<ApplicantDuplicateDto>();

            var masterId = applicant.MasterApplicantId ?? applicant.Id;

            return await _context.Applicants
                .AsNoTracking()
                .Where(a => a.Id != applicantId && (a.Email == applicant.Email || a.FullName == applicant.FullName))
                .Select(a => new ApplicantDuplicateDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    Email = a.Email,
                    PhoneNumber = a.PhoneNumber,
                    IsApplicationsOwner = a.Id == masterId 
                })
                .ToListAsync();
        }
        public IQueryable<Applicant> GetAllAsQueryable()
        {
            return _context.Applicants.AsQueryable();
        }

        public async Task<List<ApplicantApplication>> GetApplicationsByApplicantOwnerAsync(int applicantId)
        {
            var baseApplicant = await _context.Applicants
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == applicantId);

            if (baseApplicant == null)
                return new List<ApplicantApplication>();

            var masterId = baseApplicant.MasterApplicantId ?? baseApplicant.Id;

            var applicationsOwner = await _context.Applicants
                .AsNoTracking()
                .Include(a => a.Applications)
                    .ThenInclude(app => app.Vacancy)
                        .ThenInclude(v => v.Title)
                .Where(a => a.Id == masterId) 
                .SelectMany(a => a.Applications)
                .ToListAsync();

            return applicationsOwner;
        }

        public async Task<List<Applicant>> GetApplicantDuplicatesAsync(int applicantId)
        {
            var applicant = await _context.Applicants
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == applicantId);

            if (applicant == null)
                return new List<Applicant>();

            var masterId = applicant.MasterApplicantId ?? applicant.Id;

            return await _context.Applicants
                .AsNoTracking()
                .Where(a => a.Id == masterId || a.MasterApplicantId == masterId)
                .ToListAsync();
        }
    }
}
