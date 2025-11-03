using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Infrastructure.Data;

namespace Recruitment.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IGenericRepository<Country> Countries { get; }
        public IGenericRepository<Location> Locations { get; }
        public IGenericRepository<Project> Projects { get; }
        public IGenericRepository<ProjectVacancy> ProjectVacancies { get; }
        public IGenericRepository<Vacancy> Vacancies { get; }
        public IGenericRepository<Title> Titles { get; }
        public IGenericRepository<Department> Departments { get; }
        public IGenericRepository<DepartmentTitle> DepartmentTitles { get; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Countries = new GenericRepository<Country>(_context);
            Locations = new GenericRepository<Location>(_context);
            Projects = new GenericRepository<Project>(_context);
            ProjectVacancies = new GenericRepository<ProjectVacancy>(_context);
            Vacancies = new GenericRepository<Vacancy>(_context);
            Titles = new GenericRepository<Title>(_context);
            Departments = new GenericRepository<Department>(_context);
            DepartmentTitles = new GenericRepository<DepartmentTitle>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
