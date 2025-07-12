using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Hrms.Core.Entities;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            EntityConfig.SetupEntity<Address, int>(builder);

            builder.Property(x => x.AddressType).IsRequired();
            builder.Property(x => x.IdentificationId).IsRequired();
            builder.Property(x => x.Line1).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Line2).HasMaxLength(250);
            builder.Property(x => x.Landmark).HasMaxLength(250);
            builder.Property(x => x.CityId).IsRequired();
            builder.Property(x => x.PinCode).IsRequired().HasMaxLength(6);
        }
    }
}
