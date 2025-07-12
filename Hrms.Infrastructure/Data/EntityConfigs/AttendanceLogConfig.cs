using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class AttendanceLogConfig : IEntityTypeConfiguration<AttendanceLog>
    {
        public void Configure(EntityTypeBuilder<AttendanceLog> builder)
        {
            builder.ToTable("AttendancesLogs").HasKey(x => x.Id);

            EntityConfig.SetupEntityBase<AttendanceLog, int>(builder);

            builder.Property(x => x.Id);
            builder.Property(x => x.AttendanceId).IsRequired();
            builder.Property(x => x.InTime).IsRequired();
            builder.Property(x => x.OutTime);
            builder.Property(x => x.Latitude).IsRequired();
            builder.Property(x => x.Longitude).IsRequired();
            builder.Property(x => x.Note).HasMaxLength(250);
        }
    }
}
