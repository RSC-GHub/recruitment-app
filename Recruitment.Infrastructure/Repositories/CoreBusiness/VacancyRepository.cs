using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Common;
using Recruitment.Application.DTOs.CoreBusiness.Vacancy;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Enums;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories.CoreBusiness
{
    public class VacancyRepository : GenericRepository<Vacancy>, IVacancyRepository
    {
        public VacancyRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }
       
        public async Task<Vacancy?> GetVacancyByIdForApi(int id)
        {
            var vacancy = await _context.Vacancies
                   .IgnoreQueryFilters()
                   .FirstOrDefaultAsync(v => v.Id == id);
            return vacancy;
        }
        
        public async Task<Vacancy?> GetVacancyByIdAsync(int id)
        {
            return await _context.Vacancies
                .Include(v => v.Title)
                .Include(v => v.ProjectVacancies!)
                    .ThenInclude(pv => pv.Project)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<List<Vacancy>> GetAllOpenedVacancies()
        {
            return await _context.Vacancies
                .Include(v => v.Title)
                .Where(v => v.Status == VacancyStatus.Open)
                .AsNoTracking()
                .ToListAsync();
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

        public async Task<PagedResult<Vacancy>> SearchAsync(
            string? search,
            int? titleId,
            int? projectId,
            VacancyStatus? status,
            int page,
            int pageSize)
        {
            var query = _context.Vacancies
                .Include(v => v.Title)
                .Include(v => v.ProjectVacancies)
                    .ThenInclude(pv => pv.Project)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(v =>
                    v.Title!.Name.ToLower().Contains(search) ||
                    v.JobDescription.ToLower().Contains(search) ||
                    v.Requirements.ToLower().Contains(search) ||
                    v.Responsibilities.ToLower().Contains(search) ||
                    v.Benefits.ToLower().Contains(search)
                );
            }

            if (titleId.HasValue)
                query = query.Where(v => v.TitleId == titleId);

            if (projectId.HasValue)
                query = query.Where(v => v.ProjectVacancies.Any(pv => pv.ProjectId == projectId));

            if (status.HasValue)
                query = query.Where(v => v.Status == status);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(v => v.Deadline)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Vacancy>(items, totalCount, page, pageSize);
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

        public async Task<List<Vacancy>> GetAllOpenedVacanciesCards()
        {
            return await _context.Vacancies
                .IgnoreQueryFilters()
                .Where(v => v.Status == VacancyStatus.Open)
                .Include(v => v.Title!)
                    .ThenInclude(t => t.DepartmentTitles!)
                        .ThenInclude(dt => dt.Department)
                .Include(v => v.ProjectVacancies!)
                    .ThenInclude(pv => pv.Project!)
                        .ThenInclude(p => p.Location)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<VacancyPositionsChartDTO>> GetVacanciesPositionsChartAsync()
        {
            return await _context.Vacancies
                .Include(v => v.Title)
                .Where(v => v.Status == VacancyStatus.Open)
                .Select(v => new VacancyPositionsChartDTO
                {
                    VacancyTitle = v.Title.Name,
                    PositionCount = v.PositionCount
                })
                .ToListAsync();
        }

        public void RemoveProjectVacancies(IEnumerable<ProjectVacancy> pvs)
        {
            var ids = pvs.Select(pv => new { pv.VacancyId, pv.ProjectId }).ToList();

            var toDelete = _context.Set<ProjectVacancy>()
                .IgnoreQueryFilters()
                .Where(pv => ids.Select(x => x.VacancyId).Contains(pv.VacancyId) &&
                             ids.Select(x => x.ProjectId).Contains(pv.ProjectId))
                .ToList();

            _context.Set<ProjectVacancy>().RemoveRange(toDelete);
        }
    }
}
