using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Hrms.Infrastructure.Data.EntityConfigs
{
    public class EducationConfig : IEntityTypeConfiguration<Education>
    {
        public void Configure(EntityTypeBuilder<Education> builder)
        {
            builder.ToTable("EduactionDetails");
            EntityConfig.SetupEntityBase<Education, int>(builder);

            builder.Property(x => x.EmployeeId).IsRequired();
            builder.Property(x => x.QualificationTypeId).IsRequired();
            builder.Property(x => x.CourseTypeId).IsRequired();
            builder.Property(x => x.CourseName).IsRequired().HasMaxLength(240);
            builder.Property(x => x.Stream).HasMaxLength(100);
            builder.Property(x => x.CollegeName).HasMaxLength(250);
            builder.Property(x => x.UniversityName).HasMaxLength(250);
            builder.Property(x => x.Start).IsRequired();
            builder.Property(x => x.End);
        }
    }
}
