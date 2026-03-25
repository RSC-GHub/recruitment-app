using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recruitment.Infrastructure.Data.Configurations.CoreBusiness
{
    public class UserProjectConfiguration : IEntityTypeConfiguration<UserProject>
    {
        public void Configure(EntityTypeBuilder<UserProject> builder)
        {
            builder.ToTable("UserProjects");

            builder.HasKey(up => new { up.UserId, up.ProjectId });

            builder.HasOne(up => up.User)
                   .WithMany(u => u.UserProjects)
                   .HasForeignKey(up => up.UserId)
                   .IsRequired(false);

            builder.HasOne(up => up.Project)
                   .WithMany(p => p.UserProjects)
                   .HasForeignKey(up => up.ProjectId)
                   .IsRequired(false);
        }
    }
}
