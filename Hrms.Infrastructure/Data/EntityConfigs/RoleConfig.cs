using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class RoleConfig : IEntityTypeConfiguration<Role<int>>
    {
        public void Configure(EntityTypeBuilder<Role<int>> builder)
        {
            builder.ToTable("Roles").HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.NormalizedName).HasMaxLength(50);
        }
    }
}
