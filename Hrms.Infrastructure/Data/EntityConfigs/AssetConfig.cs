using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class AssetConfig : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.ToTable("Assets");

            EntityConfig.SetupEntityWithStatusTracking<Asset, int>(builder);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.IsInWarranty).IsRequired();
            builder.Property(x => x.WarrantyPeriod).IsRequired();
            builder.Property(x => x.PurchaseDate).IsRequired();
            builder.Property(x => x.SerialNumber).IsRequired().HasMaxLength(100);
            builder.Property(x => x.VendorName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.VariantId).IsRequired();
            builder.Property(x => x.AssetTypeId).IsRequired();
            builder.Property(x => x.ManufacturerId).IsRequired();
            builder.HasOne(x => x.Variant).WithMany().HasForeignKey(x => x.VariantId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Manufacturer).WithMany().HasForeignKey(x => x.ManufacturerId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.AssetType).WithMany().HasForeignKey(x => x.AssetTypeId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
