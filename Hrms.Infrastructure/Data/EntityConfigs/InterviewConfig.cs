using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Hrms.Core.Entities;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class InterviewConfig : IEntityTypeConfiguration<Interview>
    {
        public void Configure(EntityTypeBuilder<Interview> builder)
        {
            builder.ToTable("Interviews");

            EntityConfig.SetupEntityBase<Interview, int>(builder);
            builder.Property(x => x.InterviewType).IsRequired();
            builder.Property(x => x.InterviewMode).IsRequired();
            builder.Property(x => x.ScheduleDate);
            builder.Property(x => x.InterviewDate).IsRequired(false);
            builder.Property(x => x.Rating).IsRequired(false);
            builder.Property(x => x.EligibleForNextRound).IsRequired(false);
            builder.Property(x => x.Remark).HasMaxLength(1000);
            builder.Property(x => x.ScheduleTime);
            builder.Property(x => x.CandidateId).IsRequired();
            builder.Property(x => x.InterviewerId).IsRequired();
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.InterviewerId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
