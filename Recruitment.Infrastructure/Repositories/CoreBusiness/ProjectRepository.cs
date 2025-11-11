using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories.CoreBusiness
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor) { }

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
            return await _context.Projects
                                 .Include(p => p.Location)
                                 .ToListAsync();
        }
    }
}
