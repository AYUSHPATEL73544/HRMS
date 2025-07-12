using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class RelationshipConfig : IEntityTypeConfiguration<Relationship>
    {
        public void Configure(EntityTypeBuilder<Relationship> builder)
        {
            EntityConfig.SetupEntityWithStatusTracking<Relationship, int>(builder);

            builder.ToTable("Relationships").HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(40);
        }
    }
}
