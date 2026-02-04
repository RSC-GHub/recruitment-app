using Microsoft.AspNetCore.Http;
using Recruitment.Application.Interfaces.Persistence;
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
using Recruitment.Infrastructure.Data;
using Recruitment.Infrastructure.Repositories.Audit;
using Recruitment.Infrastructure.Repositories.CoreBusiness;
using Recruitment.Infrastructure.Repositories.RecruitmentProcess;
using Recruitment.Infrastructure.Repositories.Reports;
using Recruitment.Infrastructure.Repositories.UserManagement;

namespace Recruitment.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        // Core Business Repositories

        public IGenericRepository<AuditLog> AuditLog { get; set; }
        public IGenericRepository<Country> Countries { get; }
        public IGenericRepository<Location> Locations { get; }
        public IGenericRepository<Project> Projects { get; }
        public IGenericRepository<ProjectVacancy> ProjectVacancies { get; }
        public IGenericRepository<Vacancy> Vacancies { get; }
        public IGenericRepository<Title> Titles { get; }
        public IGenericRepository<Department> Departments { get; }
        public IGenericRepository<DepartmentTitle> DepartmentTitles { get; }

        public IGenericRepository<Permission> Permissions { get; }
        public IGenericRepository<RolePermission> RolePermissions { get; set; }
        public IGenericRepository<Currency> Currencies { get; }
        public IGenericRepository<RejectionReason> RejectionReasons { get; }
        public IGenericRepository<Interviewer> Interviewers { get; set; }

        // Report Repository
        //public IGenericRepository<Report> Reports { get; }
        public IRoleRepository Roles { get; }
        public IVacancyRepository VacancyRepository { get; }
        public IProjectRepository ProjectRepository { get; }
        public ITitleRepository TitleRepository { get; }
        public IApplicantRepository ApplicantRepository { get; }
        public IApplicantApplicationRepository ApplicationRepository { get; }
        public IInterviewRepository InterviewRepository { get; set; }
        public IAuditLogRepository AuditLogRepository { get; set; }
        public IRejectionReasonRepository RejectionReasonRepository { get; set; }
        public IDepartmentRepository DepartmentRepository { get; }

        public ILocationRepository LocationRepository { get; set; }
        public IInterviewerRepository InterviewerRepository { get; set; }
        public IReportRepository ReportsRepository { get; set; }


        public UnitOfWork(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;

            AuditLog = new GenericRepository<AuditLog>(_context, httpContextAccessor);
            Countries = new GenericRepository<Country>(_context, httpContextAccessor);
            Locations = new GenericRepository<Location>(_context, httpContextAccessor);
            Projects = new GenericRepository<Project>(_context, httpContextAccessor);
            ProjectVacancies = new GenericRepository<ProjectVacancy>(_context, httpContextAccessor);
            Vacancies = new GenericRepository<Vacancy>(_context, httpContextAccessor);
            Titles = new GenericRepository<Title>(_context, httpContextAccessor);
            Departments = new GenericRepository<Department>(_context, httpContextAccessor);
            DepartmentTitles = new GenericRepository<DepartmentTitle>(_context, httpContextAccessor);
            RejectionReasons = new GenericRepository<RejectionReason>(_context, httpContextAccessor);
            Interviewers = new GenericRepository<Interviewer>(_context, httpContextAccessor);

            Roles = new RoleRepository(_context);
            Permissions = new GenericRepository<Permission>(_context, httpContextAccessor);
            RolePermissions = new GenericRepository<RolePermission>(_context, httpContextAccessor);

            VacancyRepository = new VacancyRepository(context, httpContextAccessor);
            ProjectRepository = new ProjectRepository(context, httpContextAccessor);
            TitleRepository = new TitleRepository(context, httpContextAccessor);
            Currencies = new GenericRepository<Currency>(_context, httpContextAccessor);
            ApplicantRepository = new ApplicantRepository(context, httpContextAccessor);
            ApplicationRepository = new ApplicantApplicationRepository(context, httpContextAccessor);
            InterviewRepository = new InterviewRepository(context, httpContextAccessor);
            AuditLogRepository = new AuditLogRepository(context, httpContextAccessor);
            RejectionReasonRepository = new RejectionReasonRepository(context, httpContextAccessor);
            LocationRepository = new LocationRepository(context, httpContextAccessor);
            DepartmentRepository = new DepartmentRepository(context, httpContextAccessor);
            InterviewerRepository = new InterviewerRepository(context, httpContextAccessor);

            //Reports = new GenericRepository<Report>(_context, httpContextAccessor);
            ReportsRepository = new ReportRepository(context, httpContextAccessor);

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
