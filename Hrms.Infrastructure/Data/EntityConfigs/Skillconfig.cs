using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class Skillconfig : IEntityTypeConfiguration<Skill>
    {

        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            builder.ToTable("Skills");

            EntityConfig.SetupEntityWithStatusTracking<Skill, int>(builder);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);



        }
    }
}
