using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class CourseTypeConfig : IEntityTypeConfiguration<CourseType>
    {
        public void Configure(EntityTypeBuilder<CourseType> builder)
        {
            EntityConfig.SetupEntityWithStatusTracking<CourseType, int>(builder);

            builder.ToTable("CourseTypes").HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(40);

            builder.HasMany(x => x.Educations).WithOne().HasForeignKey(x => x.CourseTypeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Candidates).WithOne().HasForeignKey(x => x.CourseTypeId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
