using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class AssetTypeConfig : IEntityTypeConfiguration<AssetType>
    {

        public void Configure(EntityTypeBuilder<AssetType> builder)
        {
            builder.ToTable("AssetTypes");

            EntityConfig.SetupEntityWithStatusTracking<AssetType, int>(builder);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.HasMany(x => x.Manufacturers).WithOne().HasForeignKey(x => x.AssetTypeId);
        }
    }
}
