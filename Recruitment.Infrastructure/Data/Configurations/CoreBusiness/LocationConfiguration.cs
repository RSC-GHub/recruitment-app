using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Infrastructure.Data.Configurations.CoreBusiness
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder.ToTable("Locations");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(l => l.Country)
                   .WithMany(c => c.Locations)
                   .HasForeignKey(l => l.CountryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
