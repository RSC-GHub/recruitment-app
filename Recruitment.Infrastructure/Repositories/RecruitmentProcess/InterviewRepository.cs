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
            var query = _context.Interviews
                .Include(i => i.Application)
                    .ThenInclude(a => a.Applicant)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Vacancy)  
                .Include(i => i.Interviewer) 
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
                .Include(i => i.Interviewer)
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
            InterviewCategory? category,
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
                    .Include(i => i.Interviewer)
                    .AsQueryable();

            // Text Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(i =>
                    (i.Interviewer != null && i.Interviewer.Name.Contains(search)) ||
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

            if (category.HasValue)
                query = query.Where(i => i.InterviewCategory == category.Value);

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
                .OrderByDescending(i => i.CreatedOn)
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
                        .ThenInclude(v => v.ProjectVacancies!)
                            .ThenInclude(pv => pv.Project)
                .Include(i => i.Interviewer)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> UpdateInterviewResultAsync(int id, InterviewResult result, string? feedback, string? Note)
        {
            var interview = await GetWithApplicationIdAsync(id);
            if (interview == null)
                return false;

            interview.InterviewResult = result;
            interview.InterviewStatus = InterviewStatus.Completed;

            switch (result)
            {
                case InterviewResult.Accepted:
                    interview.Application.ApplicationStatus = ApplicationStatus.AcceptedInterview;
                    break;
                case InterviewResult.SecondChoice:
                    interview.Application.ApplicationStatus = ApplicationStatus.Pending;
                    break;
                case InterviewResult.Rejected:
                    interview.Application.ApplicationStatus = ApplicationStatus.Rejected;
                    break;
                default:
                    break; 
            }

            interview.Feedback = feedback;
            interview.InterViewNote = Note;

            _context.Interviews.Update(interview);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateInterviewAsync(
            int id,
            int interviewerId,
            InterviewStatus interviewStatus,
            InterviewType interviewType,
            int durationMinutes,
            string? interviewNote
                )
        {
            var interview = await GetWithApplicationIdAsync(id);
            if (interview == null)
                return false;

            interview.InterviewerId = interviewerId;
            interview.InterviewStatus = interviewStatus;
            interview.InterviewType = interviewType;
            interview.DurationMinutes = durationMinutes;
            interview.InterViewNote = interviewNote;

            if (interviewStatus == InterviewStatus.Cancelled || interviewStatus == InterviewStatus.Postponed)
            {
                interview.InterviewResult = InterviewResult.Pending;
                interview.Application.ApplicationStatus = ApplicationStatus.InterviewOnHold;
            }

            _context.Interviews.Update(interview);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> CountTodaysInterviewsAsync()
        {
            return await _context.Interviews
                .CountAsync(i => i.ScheduledDate.Date == DateTime.UtcNow.Date);
        }

        public async Task<int> CountPendingInterviewResultsAsync()
        {
            return await _context.Interviews
                .CountAsync(i =>
                    i.ScheduledDate < DateTime.UtcNow &&
                    i.InterviewResult == InterviewResult.Pending && i.InterviewStatus == InterviewStatus.Scheduled);
        }

        public async Task<Interview?> GetByIdWithApplicantAsync(int id)
        {
            return await _context.Interviews
                .Include(i => i.Application)
                    .ThenInclude(a => a.Applicant)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Vacancy)
                        .ThenInclude(v => v.Title)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Interview>> GetAllForCalendarAsync(int? month = null, int? year = null)
        {
            var query = _context.Interviews
                .Include(i => i.Application)
                    .ThenInclude(a => a.Applicant)
                .Include(i => i.Application)
                    .ThenInclude(a => a.Vacancy)
                        .ThenInclude(v => v.Title)
                .AsQueryable();

            query = query.Where(i => i.InterviewStatus == InterviewStatus.Scheduled);

            if (month.HasValue)
                query = query.Where(i => i.ScheduledDate.Month == month.Value);

            if (year.HasValue)
                query = query.Where(i => i.ScheduledDate.Year == year.Value);

            return await query.ToListAsync();
        }

    }
}
