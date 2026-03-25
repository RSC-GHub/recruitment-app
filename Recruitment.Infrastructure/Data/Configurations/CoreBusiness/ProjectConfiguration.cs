using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Infrastructure.Data.Configurations.CoreBusiness
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.ProjectName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.Status)
                   .HasConversion<string>()
                   .IsRequired()
                   .HasMaxLength(20);

            builder.HasOne(p => p.Location)
                   .WithMany(l => l.Projects)
                   .HasForeignKey(p => p.LocationId)
                   .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
