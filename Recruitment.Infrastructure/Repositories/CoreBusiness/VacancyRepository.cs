using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Enums;
using Recruitment.Infrastructure.Data;
using Recruitment.Application.Common;

namespace Recruitment.Infrastructure.Repositories.CoreBusiness
{
    public class VacancyRepository : GenericRepository<Vacancy>, IVacancyRepository
    {
        public VacancyRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }
       
        public async Task<Vacancy?> GetVacancyByIdAsync(int id)
        {
            return await _context.Vacancies
                .Include(v => v.Title)
                .Include(v => v.ProjectVacancies!)
                    .ThenInclude(pv => pv.Project)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<List<Vacancy>> GetAllVacanciesWithProjectsAsync()
        {
            return await _context.Vacancies
                .Include(v => v.Title)
                .Include(v => v.ProjectVacancies!)
                    .ThenInclude(pv => pv.Project)
                .ToListAsync();
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeId = null)
        {
            name = name.Trim().ToLower();

            return await _context.Vacancies.AnyAsync(v =>
                v.Title!.Name.ToLower() == name &&
                (!excludeId.HasValue || v.Id != excludeId.Value)
            );
        }

        public async Task<List<Vacancy>> SearchAsync(string? keyword)
        {
            keyword = keyword?.Trim().ToLower();

            return await _context.Vacancies
                .Include(v => v.Title)
                .Include(v => v.ProjectVacancies!)
                    .ThenInclude(pv => pv.Project)
                .Where(v =>
                    string.IsNullOrEmpty(keyword) ||
                    v.JobDescription.ToLower().Contains(keyword) ||
                    v.Requirements.ToLower().Contains(keyword) ||
                    v.Responsibilities.ToLower().Contains(keyword) ||
                    v.Benefits.ToLower().Contains(keyword) ||
                    v.Title!.Name.ToLower().Contains(keyword)
                )
                .ToListAsync();
        }

        public async Task<List<Vacancy>> FilterAsync(
            int? titleId,
            int? projectId,
            VacancyStatus? status)
        {
            var query = _context.Vacancies
                .Include(v => v.Title)
                .Include(v => v.ProjectVacancies!)
                    .ThenInclude(pv => pv.Project)
                .AsQueryable();

            if (titleId.HasValue)
                query = query.Where(v => v.TitleId == titleId);

            if (projectId.HasValue)
                query = query.Where(v =>
                    v.ProjectVacancies!.Any(pv => pv.ProjectId == projectId));

            if (status.HasValue)
                query = query.Where(v => v.Status == status);

            return await query.ToListAsync();
        }

        public async Task<PagedResult<Vacancy>> GetPagedAsync(int page, int pageSize)
        {
            var totalCount = await _context.Vacancies.CountAsync();

            var items = await _context.Vacancies
                .Include(v => v.Title)
                .Include(v => v.ProjectVacancies!)
                    .ThenInclude(pv => pv.Project)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Vacancy>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Vacancies.CountAsync();
        }

        public async Task<Vacancy?> GetForEditAsync(int id)
        {
            return await _context.Vacancies
                .Include(v => v.Title)
                .Include(v => v.ProjectVacancies!)
                    .ThenInclude(pv => pv.Project)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}
