using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace Hrms.Infrastructure.Data.EntityConfigs
{
    public class EmployeeAttendanceRuleConfig : IEntityTypeConfiguration<EmployeeAttendanceRule>
    {
        public void Configure(EntityTypeBuilder<EmployeeAttendanceRule> builder)
        {
            builder.ToTable("EmployeeAttendanceRule");

            EntityConfig.SetupEntityBase<EmployeeAttendanceRule, int>(builder);

            builder.Property(x => x.EmployeeId).IsRequired();
            builder.Property(x => x.AttendanceRuleId).IsRequired();
        }
    }
}
