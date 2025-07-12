using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class AssetAllocationConfig : IEntityTypeConfiguration<AssetAllocation>
    {
        public void Configure(EntityTypeBuilder<AssetAllocation> builder)
        {
            builder.ToTable("AssetAllocations").HasKey(x => x.Id);

            EntityConfig.SetupEntityBase<AssetAllocation, int>(builder);

            builder.Property(x => x.AssetId).IsRequired();
            builder.Property(x => x.EmployeeId).IsRequired();
            builder.HasOne(x => x.Asset).WithMany().HasForeignKey(x => x.AssetId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Employee).WithMany().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
