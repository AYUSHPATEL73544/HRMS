using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class DesignationConfig : IEntityTypeConfiguration<Designation>
    {
        public void Configure(EntityTypeBuilder<Designation> builder)
        {
            builder.ToTable("Designations");

            EntityConfig.SetupEntityWithStatusTracking<Designation, int>(builder);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);

            builder.HasMany(x => x.Employees).WithOne().HasForeignKey(x => x.DesignationId);
            builder.HasMany(x => x.WorkHistories).WithOne().HasForeignKey(x => x.DesignationId);
        }
    }
}
