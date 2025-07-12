using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Hrms.Core.Entities;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class AttendanceConfig : IEntityTypeConfiguration<Attendance>
    {
        public void Configure(EntityTypeBuilder<Attendance> builder)
        {
            builder.ToTable("Attendances");

            EntityConfig.SetupEntityWithStatusTracking<Attendance, int>(builder);

            builder.Property(x => x.EmployeeId).IsRequired();
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.DayOfWeek).IsRequired();
        }
    }
}
