using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Common;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Entities.RecruitmentProccess;
using Recruitment.Domain.Enums;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories.CoreBusiness
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor) { }


        private async Task<PagedResult<Project>> ToPagedResultAsync(
            IQueryable<Project> query,
            int page,
            int pageSize)
        {
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Project>(
                items,
                totalCount,
                page,
                pageSize
            );
        }

        public async Task<PagedResult<Project>> GetPagedAsync(
            int page,
            int pageSize,
            string? search = null,
            int? countryId = null)
        {
            var query = _context.Projects
                .Include(p => p.Location)
                .ThenInclude(l => l.Country)
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.ProjectName.Contains(search));
            }

            if (countryId.HasValue)
            {
                query = query.Where(p =>
                    p.Location != null &&
                    p.Location.CountryId == countryId.Value);
            }

            var totalCount = await query.CountAsync();

            var projects = await query
                .OrderBy(p => p.CreatedOn)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            //return new PagedResult<Project>(
            //    projects,
            //    totalCount,
            //    page,
            //    pageSize
            //);
            return await ToPagedResultAsync(query, page, pageSize);
        }

        public async Task<Project?> GetProjectWithVacanciesAsync(int projectId)
        {
            return await _context.Projects
                .Include(p => p.ProjectVacancies)
                    .ThenInclude(pv => pv.Vacancy)
                        .ThenInclude(v => v.Title)
                .Include(p => p.Location)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<IEnumerable<Project>> GetProjectsByLocationAsync(int locationId)
        {
            return await _context.Projects
                .Where(p => p.LocationId == locationId)
                .Include(p => p.ProjectVacancies)
                    .ThenInclude(pv => pv.Vacancy)
                .ToListAsync();
        }

        public async Task<Project?> GetProjectWithLocationAsync(int projectId)
        {
            return await _context.Projects
                                 .Include(p => p.Location)
                                 .FirstOrDefaultAsync(p => p.Id == projectId);
        }
        public async Task<IEnumerable<Project>> GetAllProjectWithLocationAsync()
        {
            var projects = await _context.Projects
                                         .AsNoTracking()
                                         .ToListAsync(); 

            var projectIds = projects.Select(p => p.LocationId).ToList();

            var locations = await _context.Locations
                                          .IgnoreQueryFilters()
                                          .Where(l => projectIds.Contains(l.Id))
                                          .ToListAsync();

            foreach (var project in projects)
            {
                var loc = locations.FirstOrDefault(l => l.Id == project.LocationId && !l.IsDeleted);

                project.Location = new Location
                {
                    Name = loc?.Name ?? ""   
                };
            }

            return projects;
        }

        public async Task<IEnumerable<Project>> GetAllActiveProjectsAsync()
        {
            var openProjects = await _context.Projects
                                             .Where(p => p.Status == ProjectStatus.Active)
                                             .AsNoTracking()
                                             .ToListAsync();
            return openProjects;
        }
    }
}
