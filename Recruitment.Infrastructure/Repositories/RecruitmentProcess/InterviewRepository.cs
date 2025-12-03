using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Common;
using Recruitment.Application.Interfaces.Persistence.RecruitmentProcess;
using Recruitment.Domain.Entities.RecruitmentProccess;
using Recruitment.Domain.Enums;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories.RecruitmentProcess
{
    public class InterviewRepository : GenericRepository<Interview>, IInterviewRepository
    {
        public InterviewRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        public async Task<PagedResult<Interview>> GetPagedAsync(int page, int pageSize)
        {
            var query = _context.Interviews.Include(i => i.Application)
                    .ThenInclude(i => i.Applicant)
                .Include(i => i.Application)
                    .ThenInclude(i => i.Vacancy.Title)
                .AsNoTracking();
            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(i => i.ScheduledDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Interview>(items, totalCount, page, pageSize);
        }

        public async Task<PagedResult<Interview>> GetByApplicationIdAsync(int applicationId, int page, int pageSize)
        {
            var query = _context.Interviews
                .Include(i => i.Application)
                .Where(i => i.ApplicationId == applicationId)
                .AsNoTracking();

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(i => i.ScheduledDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Interview>(items, totalCount, page, pageSize);
        }
        public async Task<Interview> GetWithApplicationIdAsync(int id)
        {
            var interview = await _context.Interviews
                .Include(i => i.Application)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);

            return interview!;
        }


        public async Task<List<Interview>> GetAllByApplicationIdAsync(int applicationId)
        {
            return await _context.Interviews
                .Where(i => i.ApplicationId == applicationId)
                .ToListAsync();
        }


        public async Task<PagedResult<Interview>> SearchAsync(
            string? search,
            InterviewStatus? status,
            InterviewResult? result,
            InterviewType? type,
            DateTime? fromDate,
            DateTime? toDate,
            int page,
            int pageSize)
        {
            var query = _context.Interviews
                    .Include(i => i.Application)
                        .ThenInclude(a => a.Applicant)
                    .Include(i => i.Application)
                        .ThenInclude(a => a.Vacancy)
                            .ThenInclude(v => v.Title)
                    .AsQueryable();

            // Text Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(i =>
                    (i.InterViewer != null && i.InterViewer.Contains(search)) ||
                    i.Application.Applicant.FullName.Contains(search) ||
                    i.Application.Vacancy.Title!.Name.Contains(search)
                );
            }

            if (status.HasValue)
                query = query.Where(i => i.InterviewStatus == status.Value);

            if (result.HasValue)
                query = query.Where(i => i.InterviewResult == result.Value);

            if (type.HasValue)
                query = query.Where(i => i.InterviewType == type.Value);

            if (fromDate.HasValue && toDate.HasValue)
            {
                query = query.Where(i =>
                    i.ScheduledDate >= fromDate.Value &&
                    i.ScheduledDate <= toDate.Value);
            }
            else if (fromDate.HasValue)
            {
                query = query.Where(i => i.ScheduledDate >= fromDate.Value);
            }
            else if (toDate.HasValue)
            {
                query = query.Where(i => i.ScheduledDate <= toDate.Value);
            }


            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(i => i.ScheduledDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Interview>(items, totalCount, page, pageSize);
        }



        public async Task<Interview?> GetWithApplicationAsync(int id)
        {
            return await _context.Interviews
                .Include(i => i.Application)
                .FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<Interview?> GetWithRelatedDataAsync(int id)
        {
            return await _context.Interviews
                .Include(i => i.Application)
                    .ThenInclude(a => a.Applicant)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Vacancy)
                        .ThenInclude(v => v.Title)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Vacancy)
                        .ThenInclude(v => v.ProjectVacancies)
                            .ThenInclude(pv => pv.Project)
                .FirstOrDefaultAsync(i => i.Id == id);
        }


        public async Task<bool> UpdateInterviewResultAsync(int id, InterviewResult result, string? feedback, string? Note)
        {
            var interview = await GetWithApplicationIdAsync(id);
            if (interview == null)
                return false;

            interview.InterviewResult = result;
            if (result == InterviewResult.Rejected)
            {
                interview.Application.ApplicationStatus = ApplicationStatus.Rejected;
            }
            else if (result == InterviewResult.SecondChoice)
            {
                interview.Application.ApplicationStatus = ApplicationStatus.SecondChoice;
            }

            interview.Feedback = feedback;
            interview.InterViewNote = Note;

            interview.InterviewStatus = InterviewStatus.Completed;

            _context.Interviews.Update(interview);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateInterviewAsync(int id, string? interViewer,
                                              InterviewStatus interviewStatus,
                                             int durationMinutes, string? interviewNote)
        {
            var interview = await GetWithApplicationIdAsync(id);
            if (interview == null)
                return false;

            interview.InterViewer = interViewer;
            interview.InterviewStatus = interviewStatus;

            if (interviewStatus == InterviewStatus.Cancelled)
            {
                interview.InterviewResult = InterviewResult.NoResult;
                interview.Application.ApplicationStatus = ApplicationStatus.InterviewCancelled;
            }

            interview.DurationMinutes = durationMinutes;
            interview.InterViewNote = interviewNote;

            _context.Interviews.Update(interview);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
