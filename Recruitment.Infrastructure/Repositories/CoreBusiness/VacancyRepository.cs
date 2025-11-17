using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories.CoreBusiness
{
    public class VacancyRepository : GenericRepository<Vacancy>, IVacancyRepository
    {
        public VacancyRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public async Task<Vacancy?> GetVacancyWithProjectsAsync(int id)
        {
            return await _context.Set<Vacancy>()
                .Include(v => v.ProjectVacancies)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
        public async Task<List<Vacancy>> GetAllVacanciesWithProjectsAsync()
        {
            return await _context.Set<Vacancy>()
                .Include(v => v.Title)
                .Include(v => v.ProjectVacancies!)
                    .ThenInclude(pv => pv.Project)
                .ToListAsync();
        }

        public async Task<Vacancy?> GetVacancyByIdAsync(int id)
        {
            return await _context.Set<Vacancy>()
                .Include(v => v.Title)
                .Include(v => v.ProjectVacancies!)
                    .ThenInclude(pv => pv.Project)
                .FirstOrDefaultAsync(v => v.Id == id);
        }
    }
}
