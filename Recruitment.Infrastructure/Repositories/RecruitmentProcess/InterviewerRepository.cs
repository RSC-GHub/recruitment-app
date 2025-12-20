using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Common;
using Recruitment.Application.Interfaces.Persistence.RecruitmentProcess;
using Recruitment.Domain.Entities.RecruitmentProccess;
using Recruitment.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recruitment.Infrastructure.Repositories.RecruitmentProcess
{
    public class InterviewerRepository
        : GenericRepository<Interviewer>, IInterviewerRepository
    {
        public InterviewerRepository(
            ApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        private async Task<PagedResult<Interviewer>> ToPagedResultAsync(
            IQueryable<Interviewer> query,
            int page,
            int pageSize)
        {
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Interviewer>(
                items,
                totalCount,
                page,
                pageSize
            );
        }

        public async Task<PagedResult<Interviewer>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null)
        {
            var query = _context.Interviewers
                    .Include(i => i.Department)
                    .AsQueryable();

            query = query.AsNoTracking();

            // Search by interviewer name
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(i =>
                    i.Name.Contains(search));
            }

            query = query.OrderBy(i => i.CreatedOn);

            return await ToPagedResultAsync(query, page, pageSize);
        }
    }
}
