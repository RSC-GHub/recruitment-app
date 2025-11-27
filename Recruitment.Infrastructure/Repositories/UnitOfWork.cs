using Microsoft.AspNetCore.Http;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Entities.UserManagement;
using Recruitment.Infrastructure.Data;
using Recruitment.Infrastructure.Repositories.CoreBusiness;

namespace Recruitment.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        // Core Business Repositories
        public IGenericRepository<Country> Countries { get; }
        public IGenericRepository<Location> Locations { get; }
        public IGenericRepository<Project> Projects { get; }
        public IGenericRepository<ProjectVacancy> ProjectVacancies { get; }
        public IGenericRepository<Vacancy> Vacancies { get; }
        public IGenericRepository<Title> Titles { get; }
        public IGenericRepository<Department> Departments { get; }
        public IGenericRepository<DepartmentTitle> DepartmentTitles { get; }

        // User Management Repositories
        public IGenericRepository<Role> Roles { get; }
        public IGenericRepository<Permission> Permissions  { get; }
        public IGenericRepository<RolePermission> RolePermissions { get; set; }
        public IGenericRepository<Currency> Currencies { get; }

        public IVacancyRepository VacancyRepository { get; }
        public IProjectRepository ProjectRepository { get; }
        public ITitleRepository TitleRepository { get; }


        public UnitOfWork(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;

            Countries = new GenericRepository<Country>(_context, httpContextAccessor);
            Locations = new GenericRepository<Location>(_context, httpContextAccessor);
            Projects = new GenericRepository<Project>(_context, httpContextAccessor);
            ProjectVacancies = new GenericRepository<ProjectVacancy>(_context, httpContextAccessor);
            Vacancies = new GenericRepository<Vacancy>(_context, httpContextAccessor);
            Titles = new GenericRepository<Title>(_context, httpContextAccessor);
            Departments = new GenericRepository<Department>(_context, httpContextAccessor);
            DepartmentTitles = new GenericRepository<DepartmentTitle>(_context, httpContextAccessor);
            Roles = new GenericRepository<Role>(_context, httpContextAccessor);
            Permissions = new GenericRepository<Permission>(_context, httpContextAccessor);
            RolePermissions = new GenericRepository<RolePermission>(_context, httpContextAccessor);

            VacancyRepository = new VacancyRepository(context, httpContextAccessor);
            ProjectRepository = new ProjectRepository(context, httpContextAccessor);
            TitleRepository = new TitleRepository(context, httpContextAccessor);
            Currencies = new GenericRepository<Currency>(_context, httpContextAccessor);
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
