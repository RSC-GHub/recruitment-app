using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.UserManagement;

namespace Recruitment.Infrastructure.Data.Configurations.UserManagement
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            // Table name
            builder.ToTable("Permissions");

            // Primary key
            builder.HasKey(p => p.Id);

            // Properties
            builder.Property(p => p.PermissionName).IsRequired().HasMaxLength(100);

            //builder.HasIndex(p => p.PermissionName)
            //    .IsUnique();

            builder.Property(p => p.Description).HasMaxLength(500);

            builder.Property(p => p.Resource).IsRequired().HasMaxLength(100);

            builder.Property(p => p.Action).IsRequired().HasMaxLength(50);

            // Relationships
            builder
                .HasMany(p => p.RolePermissions)
                .WithOne(rp => rp.Permission)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new Permission
                {
                    Id = 9,
                    PermissionName = "Add new Applicant",
                    Description = "Allowing the user to add applicants to the system",
                    Resource = "Applicant",
                    Action = "Create",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 10,
                    PermissionName = "Add new Vacancy",
                    Description = "Allowing the user to add vacancy to the system",
                    Resource = "Vacancy",
                    Action = "Create",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 11,
                    PermissionName = "Make changes to existence vacancy",
                    Description = "Allowing the user to Edit on vacancy data",
                    Resource = "Vacancy",
                    Action = "Edit",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 12,
                    PermissionName = "Assign applicants to open vacancies",
                    Description =
                        "Allowing the user to assign a job applicant in order to create application for him/her",
                    Resource = "Vacancy",
                    Action = "Assign",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 13,
                    PermissionName = "View submission for specific Vacancy",
                    Description = "Allowing the user to see who applied for this Vacancy",
                    Resource = "Vacancy",
                    Action = "ViewSubmission",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 14,
                    PermissionName = "Delete specific Vacancy",
                    Description = "Allowing the user to delete vacancies",
                    Resource = "Vacancy",
                    Action = "Delete",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 15,
                    PermissionName = "Assign vacancies to applicants",
                    Description =
                        "Allowing the user to assign a job applicant in order to create application for him/her",
                    Resource = "Applicant",
                    Action = "Assign",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 16,
                    PermissionName = "Show applicant's history",
                    Description = "Allowing the user to view specific applicant's history",
                    Resource = "Applicant",
                    Action = "ViewHistory",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 17,
                    PermissionName = "Make changes to existence Applicant's information",
                    Description = "Allowing user to edit applicant's data",
                    Resource = "Applicant",
                    Action = "Edit",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 18,
                    PermissionName = "Delete specific applicant",
                    Description = "Allowing the user to delete applicants",
                    Resource = "Applicant",
                    Action = "Delete",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 19,
                    PermissionName = "Take action about specific application",
                    Description = "Allowing the user to make changes to application's status",
                    Resource = "Application",
                    Action = "ReviewApplication",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 20,
                    PermissionName = "Register interview's final result",
                    Description = "Allowing user to submit the final result for the interviews",
                    Resource = "Interview",
                    Action = "RegisterFinalResult",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 21,
                    PermissionName = "Make changes to existence interview",
                    Description = "Allowing the user to modify interview details",
                    Resource = "Interview",
                    Action = "Edit",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 22,
                    PermissionName = "Add new project",
                    Description = "Allowing the user to add project to the system",
                    Resource = "Project",
                    Action = "Create",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 23,
                    PermissionName = "Make changes to existence Project",
                    Description = "Allowing the user to change project's name, status, or location",
                    Resource = "Project",
                    Action = "Edit",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 24,
                    PermissionName = "Add new Job title",
                    Description = "Allowing the user to add new job title to the system",
                    Resource = "Title",
                    Action = "Create",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 25,
                    PermissionName = "Make changes to existence Job title",
                    Description = "Allowing the user to edit title name or department",
                    Resource = "Title",
                    Action = "Edit",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 26,
                    PermissionName = "Add new Location",
                    Description = "Allowing the user to add new location to the system",
                    Resource = "Location",
                    Action = "Create",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 27,
                    PermissionName = "Make changes to existence location",
                    Description = "Allowing the user to edit location data",
                    Resource = "Location",
                    Action = "Edit",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 28,
                    PermissionName = "Add new Interviewer",
                    Description = "Allowing the user to add new interviewer",
                    Resource = "Interviewer",
                    Action = "Create",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 29,
                    PermissionName = "Make changes to existence interviewer",
                    Description = "Allowing the user to edit interviewer data",
                    Resource = "Interviewer",
                    Action = "Edit",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 30,
                    PermissionName = "Delete specific interviewer",
                    Description = "Allowing the user to delete interviewer",
                    Resource = "Interviewer",
                    Action = "Delete",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 31,
                    PermissionName = "Add rejection reasons to the system",
                    Description = "Allowing the user to add rejection reasons",
                    Resource = "RejectionReason",
                    Action = "Create",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 32,
                    PermissionName = "Make changes to existence rejection reason",
                    Description = "Allowing the user to edit rejection reason",
                    Resource = "RejectionReason",
                    Action = "Edit",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 33,
                    PermissionName = "Delete specific rejection reason",
                    Description = "Allowing the user to delete rejection reasons",
                    Resource = "RejectionReason",
                    Action = "Delete",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 34,
                    PermissionName = "Add new departments",
                    Description = "Allowing the user to add department",
                    Resource = "Department",
                    Action = "Create",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 35,
                    PermissionName = "View Department",
                    Description = "Allowing the user to see departments",
                    Resource = "Department",
                    Action = "View",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 36,
                    PermissionName = "Make changes to existence department's name",
                    Description = "Allowing user to edit department name",
                    Resource = "Department",
                    Action = "Edit",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 37,
                    PermissionName = "Delete specific Department",
                    Description = "Allowing user to delete department",
                    Resource = "Department",
                    Action = "Delete",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 38,
                    PermissionName = "View Locations",
                    Description = "Allowing user to view location list",
                    Resource = "Location",
                    Action = "View",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 39,
                    PermissionName = "View titles",
                    Description = "Allowing the user to view job title list",
                    Resource = "Title",
                    Action = "View",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 40,
                    PermissionName = "View Projects",
                    Description = "Allowing the user to view projects list",
                    Resource = "Project",
                    Action = "View",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 41,
                    PermissionName = "View Rejection Reasons",
                    Description = "Allowing the user to view rejection reasons list",
                    Resource = "RejectionReason",
                    Action = "View",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 42,
                    PermissionName = "View interviewers list",
                    Description = "Allowing the user to view interviewer lists",
                    Resource = "Interviewer",
                    Action = "View",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 43,
                    PermissionName = "View and manage Countries module",
                    Description = "Allowing the user to manage countries",
                    Resource = "Country",
                    Action = "View",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 44,
                    PermissionName = "View and manage Currency module",
                    Description = "Allowing the user to manage currency",
                    Resource = "Currency",
                    Action = "View",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 45,
                    PermissionName = "Add Interview for specific application in the system",
                    Description = "Allowing the user to schedule an interview",
                    Resource = "Interview",
                    Action = "Create",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 46,
                    PermissionName = "Delete Location",
                    Description = "Allowing the user to delete location",
                    Resource = "Location",
                    Action = "Delete",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                },
                new Permission
                {
                    Id = 47,
                    PermissionName = "Delete specific Title",
                    Description = "Allowing the user to delete title",
                    Resource = "Title",
                    Action = "Delete",
                    CreatedBy = "admin@rsc.com.eg",
                    CreatedOn = new DateTime(2026, 3, 24, 11, 12, 1),
                    IsDeleted = false,
                }
            );
        }
    }
}
