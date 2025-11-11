using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories.CoreBusiness
{
    public class VacancyRepository : GenericRepository<Vacancy>, IVacancyRepository
    {
        public VacancyRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
            : base(context, httpContextAccessor)
        {
        }

        public async Task<Vacancy?> GetVacancyWithProjectsAsync(int id)
        {
            return await _context.Vacancies
                .Include(v => v.Title)
                .Include(v => v.ProjectVacancies)
                    .ThenInclude(pv => pv.Project)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Vacancy>> GetAllVacanciesWithProjectsAsync()
        {
            return await _context.Vacancies
                .Include(v => v.Title)
                .Include(v => v.ProjectVacancies)
                    .ThenInclude(pv => pv.Project)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Vacancy>> GetOpenVacanciesAsync()
        {
            return await _context.Vacancies
                .Where(v => v.Status == Domain.Enums.VacancyStatus.Open)
                .Include(v => v.Title)
                .Include(v => v.ProjectVacancies)
                    .ThenInclude(pv => pv.Project)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
