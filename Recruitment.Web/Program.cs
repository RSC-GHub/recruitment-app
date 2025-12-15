using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Recruitment.Application.Interfaces.Services.File;
using Recruitment.Domain.Entities.UserManagement;
using Recruitment.Infrastructure;
using Recruitment.Infrastructure.Data;
using Recruitment.Web.Authorization;
using Recruitment.Web.Middleware.AuditTrailMiddleware;
using Recruitment.Web.Middleware.ExceptionMiddleware;
using Recruitment.Web.Services;
using System.Data;

namespace Recruitment.Web
{
    public class Program
    {
        public static async Task Main(string[] args) 
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IDbConnection>(sp =>
            new SqlConnection(
                builder.Configuration.GetConnectionString("DefaultConnection")
            ));


            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddScoped<IFileStorageService, FileService>();

            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication()
            .AddCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Custom Middleware
            app.UseGlobalExceptionHandling();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication(); 
            app.UseAuthorization();

            app.UseAuditTrail();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // ===== Seed Admin =====
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var roleManager = services.GetRequiredService<RoleManager<Role>>();
                var userManager = services.GetRequiredService<UserManager<User>>();

                const string adminRoleName = "Admin";
                const string adminEmail = "admin@rsc.com.eg";
                const string adminPassword = "Admin@123";

                if (!await roleManager.RoleExistsAsync(adminRoleName))
                {
                    var role = new Role
                    {
                        Name = adminRoleName,
                        Description = "Administrator role",
                        IsActive = true,
                        CreatedBy = "System"
                    };
                    await roleManager.CreateAsync(role);
                }


                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new User
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        FullName = "System Admin",
                        IsActive = true
                    };
                    var result = await userManager.CreateAsync(adminUser, adminPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, adminRoleName);
                    }
                }
            }

            app.Run();
        }
    }
}
