using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class CityConfig : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.ToTable("Cities");

            EntityConfig.SetupEntityWithStatusTracking<City, int>(builder);

            builder.Property(x => x.StateId).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        }
    }
}
