using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class HolidayConfig : IEntityTypeConfiguration<Holiday>
    {
        public void Configure(EntityTypeBuilder<Holiday> builder)
        {
            builder.ToTable("Holidays");

            EntityConfig.SetupEntityWithStatusTracking<Holiday, int>(builder);

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(250);
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.Year).IsRequired();
        }
    }
}
