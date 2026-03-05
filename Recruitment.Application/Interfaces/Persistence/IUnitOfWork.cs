using Recruitment.Application.Interfaces.Persistence.Audit;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Application.Interfaces.Persistence.RecruitmentProcess;
using Recruitment.Application.Interfaces.Persistence.Reports;
using Recruitment.Application.Interfaces.Persistence.UserManagement;
using Recruitment.Domain.Entities.Aduit;
using Recruitment.Domain.Entities.CoreBusiness;
using Recruitment.Domain.Entities.RecruitmentProccess;
using Recruitment.Domain.Entities.Reports;
using Recruitment.Domain.Entities.UserManagement;

namespace Recruitment.Application.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<AuditLog> AuditLog { get; }
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
        IGenericRepository<RejectionReason> RejectionReasons { get; }
        IGenericRepository<Interviewer> Interviewers { get; }

        // Report
        //IGenericRepository<Report> Reports { get; }

        IReportRepository ReportsRepository { get; }
        IProjectRepository ProjectRepository { get; }
        ITitleRepository TitleRepository { get; }
        IVacancyRepository VacancyRepository { get; }
        IApplicantRepository ApplicantRepository { get; }
        IApplicantApplicationRepository ApplicationRepository { get; }
        IInterviewRepository InterviewRepository { get; }
        IAuditLogRepository AuditLogRepository { get; }
        IRejectionReasonRepository RejectionReasonRepository { get; }
        IDepartmentRepository DepartmentRepository { get; }

        ILocationRepository LocationRepository { get; }
        IInterviewerRepository InterviewerRepository { get; }


        Task<int> CompleteAsync();
    }
}
 