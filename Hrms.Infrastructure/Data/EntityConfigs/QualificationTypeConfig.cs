using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Hrms.Core.Entities;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class QualificationTypeConfig : IEntityTypeConfiguration<QualificationType>
    {
        public void Configure(EntityTypeBuilder<QualificationType> builder)
        {
            EntityConfig.SetupEntityWithStatusTracking<QualificationType, int>(builder);

            builder.ToTable("QualificationTypes").HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(40);
          
            builder.HasMany(x => x.Educations).WithOne().HasForeignKey(x => x.QualificationTypeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Candidates).WithOne().HasForeignKey(x => x.QualificationTypeId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
