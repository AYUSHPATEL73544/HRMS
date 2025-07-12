using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class CountryConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries");

            EntityConfig.SetupEntityWithStatusTracking<Country, int>(builder);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            
        }
    }
}
