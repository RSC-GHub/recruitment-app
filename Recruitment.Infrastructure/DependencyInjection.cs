using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recruitment.Application.Interfaces.Common;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Persistence.Audit;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Application.Interfaces.Persistence.RecruitmentProcess;
using Recruitment.Application.Interfaces.Persistence.UserManagement;
using Recruitment.Application.Interfaces.Services.Audit;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;
using Recruitment.Application.Interfaces.Services.UserManagement;
using Recruitment.Application.Services.Audit;
using Recruitment.Application.Services.Common;
using Recruitment.Application.Services.CoreBusiness;
using Recruitment.Application.Services.RecruitmentProccess;
using Recruitment.Application.Services.UserManagement;
using Recruitment.Infrastructure.Data;
using Recruitment.Infrastructure.Repositories;
using Recruitment.Infrastructure.Repositories.Audit;
using Recruitment.Infrastructure.Repositories.CoreBusiness;
using Recruitment.Infrastructure.Repositories.RecruitmentProcess;
using Recruitment.Infrastructure.Repositories.UserManagement;

namespace Recruitment.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database context
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ITitleRepository, TitleRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IVacancyRepository, VacancyRepository>();
            services.AddScoped<IApplicantRepository, ApplicantRepository>();
            services.AddScoped<IApplicantApplicationRepository, ApplicantApplicationRepository>();
            services.AddScoped<IInterviewRepository, InterviewRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();

            // Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ITitleService, TitleService>();
            services.AddScoped<IDepartmentTitleService, DepartmentTitleService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IVacancyService, VacancyService>();
            services.AddScoped<ICurrencyService, CurrencyService>();
            services.AddScoped<IApplicantService, ApplicantService>();
            services.AddScoped<IApplicantApplicationService, ApplicantApplicationService>();
            services.AddScoped<IInterviewService, InterviewService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IExcelExportService, ExcelExportService>();
            services.AddScoped<IRejectionReasonService, RejectionReasonService>();

            return services;
        }
    }
}
