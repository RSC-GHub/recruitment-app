using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Recruitment.Domain.Entities.CoreBusiness;

namespace Recruitment.Infrastructure.Data.Configurations.CoreBusiness
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.Code)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.HasMany(c => c.Locations)
                   .WithOne(l => l.Country)
                   .HasForeignKey(l => l.CountryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
