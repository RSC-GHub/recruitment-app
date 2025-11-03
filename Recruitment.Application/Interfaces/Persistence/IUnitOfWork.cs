using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Application.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Country> Countries { get; }
        IGenericRepository<Location> Locations { get; }
        IGenericRepository<Project> Projects { get; }
        IGenericRepository<ProjectVacancy> ProjectVacancies { get; }
        IGenericRepository<Vacancy> Vacancies { get; }
        IGenericRepository<Title> Titles { get; }
        IGenericRepository<Department> Departments { get; }
        IGenericRepository<DepartmentTitle> DepartmentTitles { get; }

        Task<int> CompleteAsync();
    }
}
