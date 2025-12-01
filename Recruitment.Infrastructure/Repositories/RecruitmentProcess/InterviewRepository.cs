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

        public async Task<PagedResult<Interview>> SearchAsync(
            string? interviewer,
            InterviewStatus? status,
            InterviewResult? result,
            int page,
            int pageSize)
        {
            // Query with includes for related data
            var query = _context.Interviews
                .Include(i => i.Application)
                    .ThenInclude(a => a.Applicant)   
                .Include(i => i.Application)
                    .ThenInclude(a => a.Vacancy.Title)    
                .AsQueryable();

            // Filter by interviewer name if provided
            if (!string.IsNullOrWhiteSpace(interviewer))
            {
                query = query.Where(i =>
                    i.InterViewer != null && i.InterViewer.Contains(interviewer) ||
                    i.Application.Applicant.FullName.Contains(interviewer) ||
                    i.Application.Vacancy.Title.Name.Contains(interviewer)
                );
            }

            if (status.HasValue)
                query = query.Where(i => i.InterviewStatus == status.Value);

            if (result.HasValue)
                query = query.Where(i => i.InterviewResult == result.Value);

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

        public async Task<bool> UpdateInterviewResultAsync(int id, InterviewResult result, string? feedback)
        {
            var interview = await _context.Interviews.FindAsync(id);
            if (interview == null)
                return false;

            interview.InterviewResult = result;
            interview.Feedback = feedback;
            
            _context.Interviews.Update(interview);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateInterviewStatusAsync(int id, InterviewStatus status)
        {
            var interview = await _context.Interviews.FindAsync(id);
            if (interview == null)
                return false;

            interview.InterviewStatus = status;
            
            _context.Interviews.Update(interview);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
