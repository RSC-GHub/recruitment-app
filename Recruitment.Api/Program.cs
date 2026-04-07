using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Recruitment.Api.Services;
using Recruitment.Application.Interfaces.Persistence;
using Recruitment.Application.Interfaces.Persistence.CoreBusiness;
using Recruitment.Application.Interfaces.Persistence.RecruitmentProcess;
using Recruitment.Application.Interfaces.Persistence.UserManagement;
using Recruitment.Application.Interfaces.Services.CoreBusiness;
using Recruitment.Application.Interfaces.Services.File;
using Recruitment.Application.Interfaces.Services.RecruitmentProccess;
using Recruitment.Application.Interfaces.Services.UserManagement;
using Recruitment.Application.MappingProfiles;
using Recruitment.Application.Services.CoreBusiness;
using Recruitment.Application.Services.RecruitmentProccess;
using Recruitment.Application.Services.UserManagement;
using Recruitment.Domain.Entities.UserManagement;
using Recruitment.Infrastructure.Data;
using Recruitment.Infrastructure.Repositories;
using Recruitment.Infrastructure.Repositories.CoreBusiness;
using Recruitment.Infrastructure.Repositories.RecruitmentProcess;
using Recruitment.Infrastructure.Repositories.UserManagement;
using System.Data;

namespace Recruitment.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ------------------ Services ------------------

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IDbConnection>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new SqlConnection(configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 52428800;
            });

            builder.Services.AddScoped<UserManager<User>>(sp => null!);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(CoreBusinessProfile));

            // ------------------ Dependency Injection ------------------

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<ICountryService, CountryService>();
            builder.Services.AddScoped<ICurrencyService, CurrencyService>();

            builder.Services.AddScoped<IVacancyRepository, VacancyRepository>();
            builder.Services.AddScoped<IVacancyService, VacancyService>();

            builder.Services.AddScoped<IApplicantRepository, ApplicantRepository>();
            builder.Services.AddScoped<IApplicantApplicationRepository, ApplicantApplicationRepository>();
            builder.Services.AddScoped<IFileStorageService, FileService>();
            builder.Services.AddScoped<IApplicantApplicationService, ApplicantApplicationService>();
            builder.Services.AddScoped<IApplicantService, ApplicantService>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowRedSea", policy =>
                {
                    policy.WithOrigins(
                            "https://redseaconstruct.com",
                            "https://www.redseaconstruct.com",
                            "https://recruitment.rsc.com.eg",  
                            "http://localhost:3000"            
                          )
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .SetIsOriginAllowedToAllowWildcardSubdomains();
                });
            });

            var app = builder.Build();

            // ------------------ HTTP Pipeline ------------------

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowRedSea");
            //app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}