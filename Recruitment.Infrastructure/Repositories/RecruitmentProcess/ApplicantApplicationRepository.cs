using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Common;
using Recruitment.Application.Interfaces.Persistence.RecruitmentProcess;
using Recruitment.Domain.Entities.Recruitment_Proccess;
using Recruitment.Domain.Entities.UserManagement;
using Recruitment.Domain.Enums;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories.RecruitmentProcess
{
    public class ApplicantApplicationRepository : GenericRepository<ApplicantApplication>, IApplicantApplicationRepository
    {

        public ApplicantApplicationRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        private async Task<PagedResult<ApplicantApplication>> ToPagedResultAsync(IQueryable<ApplicantApplication> query, int page, int pageSize)
        {
            var totalCount = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new PagedResult<ApplicantApplication>(items, totalCount, page, pageSize);
        }

        public async Task<PagedResult<ApplicantApplication>> GetByVacancyIdAsync(int vacancyId, int page, int pageSize)
        {
            var query = _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.Vacancy)
                .Where(a => a.VacancyId == vacancyId);

            return await ToPagedResultAsync(query, page, pageSize);
        }

        public async Task<PagedResult<ApplicantApplication>> GetByApplicantIdAsync(int applicantId, int page, int pageSize)
        {
            var query = _context.Applications
                .Include(a => a.Vacancy)
                .Include(a => a.User)
                .Where(a => a.ApplicantId == applicantId);

            return await ToPagedResultAsync(query, page, pageSize);
        }

        public async Task<ApplicantApplication?> GetByApplicantAndVacancyAsync(int applicantId, int vacancyId)
        {
            return await _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.Vacancy)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.ApplicantId == applicantId && a.VacancyId == vacancyId);
        }

        public async Task<PagedResult<ApplicantApplication>> GetAllWithDetailsAsync(int page, int pageSize)
        {
            var query = _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.Vacancy)
                .Include(a => a.User);

            return await ToPagedResultAsync(query, page, pageSize);
        }

        public async Task<PagedResult<ApplicantApplication>> GetByStatusAsync(ApplicationStatus status, int page, int pageSize)
        {
            var query = _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.Vacancy)
                .Where(a => a.ApplicationStatus == status);

            return await ToPagedResultAsync(query, page, pageSize);
        }

        public async Task<PagedResult<ApplicantApplication>> GetPendingReviewAsync(int page, int pageSize)
        {
            var query = _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.Vacancy)
                .Where(a => a.ApplicationStatus == ApplicationStatus.Submitted);

            return await ToPagedResultAsync(query, page, pageSize);
        }

        public async Task<int> CountByVacancyAsync(int vacancyId)
        {
            return await _context.Applications
                .CountAsync(a => a.VacancyId == vacancyId);
        }

        public async Task AssignApplicantAsync(int applicantId, int vacancyId, int userId, string Note)
        {
            var exists = await GetByApplicantAndVacancyAsync(applicantId, vacancyId);
            if (exists != null) return;

            var application = new ApplicantApplication
            {
                ApplicantId = applicantId,
                VacancyId = vacancyId,
                ApplicationDate = DateTime.Now,
                ApplicationStatus = ApplicationStatus.Submitted,
                Note = Note,
                UserId = userId
            };

            await AddAsync(application);
        }

        public async Task ReviewApplicationAsync(int applicationId, int reviewedByUserId, ApplicationStatus status, string? note = null)
        {
            var application = await _context.Applications.FindAsync(applicationId);
            if (application == null) return;

            application.ReviewedBy = reviewedByUserId;
            application.ReviewDate = DateTime.Now;
            application.ApplicationStatus = status;
            application.Note = note;

            Update(application);
        }
    }
}
