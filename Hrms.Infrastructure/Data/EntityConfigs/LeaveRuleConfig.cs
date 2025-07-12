using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class LeaveRuleConfig : IEntityTypeConfiguration<LeaveRule>
    {
        public void Configure(EntityTypeBuilder<LeaveRule> builder)
        {

            EntityConfig.SetupEntityBase<LeaveRule, int>(builder);

            builder.ToTable("LeaveRules");

            builder.Property(x => x.CompanyId);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(50);
            builder.Property(x => x.MaxAllowedInYear).IsRequired();
            builder.Property(x => x.MaxAllowedInMonth).IsRequired();
            builder.Property(x => x.MaxAllowedContinues).IsRequired();
            builder.Property(x => x.CountWeekendAsLeave).IsRequired();
            builder.Property(x => x.CountHolidayAsLeave).IsRequired();
            builder.Property(x => x.AccrualFrequency).IsRequired();
            builder.Property(x => x.AccrualPeriod).IsRequired();
            builder.Property(x => x.CreditableOnAccrualBasis).IsRequired();
            builder.Property(x => x.AllowedUnderProbation).IsRequired();
            builder.Property(x => x.AllowedBackDatedLeaves).IsRequired();
            builder.Property(x => x.MaxBackDatedLeavesAllowed).IsRequired();
            builder.Property(x => x.AllowedNegative).IsRequired();
            builder.Property(x => x.AllowedCarryForward).IsRequired();
            builder.Property(x => x.AllowedDonation).IsRequired();
            builder.Property(x => x.ApplyTillNextYear).IsRequired();
            builder.Property(x => x.FutureDatedLeavesAllowed).IsRequired();
            builder.Property(x => x.FutureDatedLeavesAllowedUpTo).IsRequired();

            builder.HasMany(x => x.Leaves).WithOne().HasForeignKey(x => x.RuleId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.LeaveLogs).WithOne().HasForeignKey(x => x.RuleId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}

