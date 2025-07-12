using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class AttendanceRuleConfig : IEntityTypeConfiguration<AttendanceRule>
    {
        public void Configure(EntityTypeBuilder<AttendanceRule> builder)
        {
            builder.ToTable("AttendanceRules");
            //EntityConfig.SetupEntityWithStatusTracking<AttendanceRule, int>(builder);
            EntityConfig.SetupEntityBase<AttendanceRule, int>(builder);

            builder.Property(x => x.CompanyId);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(100);
            builder.Property(x => x.InTime).IsRequired();
            builder.Property(x => x.OutTime).IsRequired();
            builder.Property(x => x.FirstHalfStart).IsRequired();
            builder.Property(x => x.FirstHalfEnd).IsRequired();
            builder.Property(x => x.SecondHalfStart).IsRequired();
            builder.Property(x => x.SecondHalfEnd).IsRequired();
            builder.Property(x => x.GraceInTime).IsRequired();
            builder.Property(x => x.GraceOutTime).IsRequired();
            builder.Property(x => x.TotalBreakDuration).IsRequired();
            builder.Property(x => x.MinEffectiveDuration).IsRequired();
            builder.Property(x => x.AutoLeaveDeduction).IsRequired();
            builder.Property(x => x.MinAnomaliesForFistHalfDeduction).IsRequired();
            builder.Property(x => x.MinAnomaliesForFullDayDeduction).IsRequired();
            builder.Property(x => x.NumberOfBreaks).IsRequired();
            builder.Property(x => x.StartDay).IsRequired();
            builder.Property(x => x.EndDay).IsRequired();
        }
    }
}
