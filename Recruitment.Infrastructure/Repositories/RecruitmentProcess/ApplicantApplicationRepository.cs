using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Common;
using Recruitment.Application.Interfaces.Persistence.RecruitmentProcess;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Entities.Recruitment_Proccess;
using Recruitment.Domain.Entities.UserManagement;
using Recruitment.Domain.Enums;
using Recruitment.Infrastructure.Data;
using System.Data.SqlTypes;

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
            var items = await query.Skip((page - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            // Load Reviewer explicitly for each item
            foreach (var app in items.Where(a => a.ReviewedBy != null))
            {
                app.Reviewer = await _context.Users
                                             .IgnoreQueryFilters()
                                             .FirstOrDefaultAsync(u => u.Id == app.ReviewedBy);
            }

            return new PagedResult<ApplicantApplication>(items, totalCount, page, pageSize);
        }

        public async Task<PagedResult<ApplicantApplication>> GetByVacancyIdAsync(int vacancyId, int page, int pageSize, string? search = null)
        {
            var query = _context.Applications
                                .Include(a => a.Applicant)
                                .Include(a => a.Vacancy)
                                .Where(a => a.VacancyId == vacancyId);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a =>
                    a.Applicant.FullName.Contains(search) ||
                    a.Applicant.Email.Contains(search) ||
                    a.Applicant.PhoneNumber.Contains(search) ||
                    (a.Note != null && a.Note.Contains(search)));
            }

            query = query.OrderByDescending(a => a.ApplicationDate);

            return await ToPagedResultAsync(query, page, pageSize);
        }


        public async Task<PagedResult<ApplicantApplication>> GetByApplicantIdAsync(int applicantId, int page, int pageSize)
        {
            var query = _context.Applications
                .Include(a => a.Vacancy)
                .Include(a => a.Reviewer)
                .Where(a => a.ApplicantId == applicantId);

            return await ToPagedResultAsync(query, page, pageSize);
        }

        public async Task<ApplicantApplication?> GetApplicationWithRelatedData(int id)
        {
            var application = await _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.Vacancy)
                    .ThenInclude(v => v.Title)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application?.ReviewedBy != null)
            {
                application.Reviewer = await _context.Users
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(u => u.Id == application.ReviewedBy);
            }

            return application;
        }
        public async Task<ApplicantApplication?> GetByApplicantAndVacancyAsync(int applicantId, int vacancyId)
        {
            var application = await _context.Applications
                                            .Include(a => a.Applicant)
                                            .Include(a => a.Vacancy)
                                            .FirstOrDefaultAsync(a => a.ApplicantId == applicantId && a.VacancyId == vacancyId);

            if (application?.ReviewedBy != null)
            {
                application.Reviewer = await _context.Users
                                                     .IgnoreQueryFilters()
                                                     .FirstOrDefaultAsync(u => u.Id == application.ReviewedBy);
            }

            return application;
        }

        public async Task<PagedResult<ApplicantApplication>> GetAllWithDetailsAsync(int page, int pageSize, ApplicationStatus? status, string? search)
        {
            var query = _context.Applications
                                .Include(a => a.Applicant)
                                .Include(a => a.Vacancy).ThenInclude(v => v.Title)
                                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(a =>
                    (a.Applicant.FullName != null && a.Applicant.FullName.Contains(search)) ||
                    (a.Applicant.Email != null && a.Applicant.Email.Contains(search)) ||
                    (a.Applicant.PhoneNumber != null && a.Applicant.PhoneNumber.Contains(search)) ||
                    (a.Note != null && a.Note.Contains(search)) ||
                    (a.Vacancy.Title != null && a.Vacancy.Title.Name.Contains(search))
                );
            }

            if (status.HasValue)
            {
                query = query.Where(a => a.ApplicationStatus == status.Value);
            }

            query = query.OrderByDescending(a => a.ApplicationDate);

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

        public async Task<int> CountByStatusAsync(ApplicationStatus? status = null)
        {
            return status.HasValue
                ? await _context.Applications.CountAsync(a => a.ApplicationStatus == status.Value)
                : await _context.Applications.CountAsync();
        }

        public async Task AssignApplicantAsync(int applicantId, int vacancyId, string Note)
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

        public async Task UpdateApplicationStatusAsync(int applicationId, ApplicationStatus newStatus)
        {
            var application = await _context.Applications.FindAsync(applicationId);
            if (application == null)
                return;


            application.ApplicationStatus = newStatus;

            _context.Applications.Update(application);
            await _context.SaveChangesAsync();
        }

        public async Task<int> CountOnHoldOlderThanAsync(int days = 3)
        {
            var cutoffDate = DateTime.UtcNow.AddDays(-days);
            return await _context.Applications
        .CountAsync(a =>
            a.ApplicationStatus == ApplicationStatus.OnHold &&
            a.ModifiedOn <= cutoffDate);
        }

        public async Task<ApplicantApplication?> GetByIdWithApplicantAsync(int id)
        {
            var application = await _context.Applications
                .Include(a => a.Applicant)
                .Include(a => a.Vacancy)
                    .ThenInclude(v => v.Title)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            if (application?.ReviewedBy != null)
            {
                application.Reviewer = await _context.Users
                    .IgnoreQueryFilters()
                    .FirstOrDefaultAsync(u => u.Id == application.ReviewedBy);
            }

            return application;
        }
    }
}
