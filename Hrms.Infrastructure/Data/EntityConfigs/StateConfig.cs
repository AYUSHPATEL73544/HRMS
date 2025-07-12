using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Hrms.Core.Entities;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class StateConfig : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.ToTable("States");

            EntityConfig.SetupEntityWithStatusTracking<State, int>(builder);

            builder.Property(x => x.CountryId).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);

            builder.HasMany(x => x.Cities).WithOne().HasForeignKey(x => x.StateId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
