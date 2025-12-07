using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Application.Interfaces.Persistence.RecruitmentProcess;
using Recruitment.Application.Interfaces.Persistence.UserManagement;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Entities.UserManagement;

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
        //IGenericRepository<Role> Roles { get; }

        IRoleRepository Roles { get; }
        IGenericRepository<Permission> Permissions { get; } 
        IGenericRepository<RolePermission> RolePermissions { get; }
        IGenericRepository<Currency> Currencies { get; }

        IProjectRepository ProjectRepository { get; }
        ITitleRepository TitleRepository { get; }
        IVacancyRepository VacancyRepository { get; }
        IApplicantRepository ApplicantRepository { get; }
        IApplicantApplicationRepository ApplicationRepository { get; }
        IInterviewRepository InterviewRepository { get; }
        Task<int> CompleteAsync();
    }
}
 