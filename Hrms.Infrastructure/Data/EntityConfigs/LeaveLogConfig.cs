using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class LeaveLogConfig : IEntityTypeConfiguration<LeaveLog>
    {
        public void Configure(EntityTypeBuilder<LeaveLog> builder)
        {
            builder.ToTable("LeaveLogs");

            EntityConfig.SetupEntityBase<LeaveLog, int>(builder);

            builder.Property(x => x.EmployeeId).IsRequired();
            builder.Property(x => x.RuleId).IsRequired();
            builder.Property(x => x.StartDate).IsRequired();
            builder.Property(x => x.EndDate).IsRequired();
            builder.Property(x => x.StartHalf).IsRequired();
            builder.Property(x => x.EndHalf).IsRequired();
            builder.Property(x => x.Purpose).IsRequired();
            builder.Property(x => x.Days).IsRequired();
            builder.Property(x => x.RejectionReason).HasMaxLength(250);
        }
    }
}
