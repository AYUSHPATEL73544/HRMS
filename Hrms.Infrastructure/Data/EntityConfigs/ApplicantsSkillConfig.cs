using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;



namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class ApplicantsSkillConfig : IEntityTypeConfiguration<ApplicantsSkill>
    {

        public void Configure(EntityTypeBuilder<ApplicantsSkill> builder)
        {
            builder.ToTable("ApplicantSkills");

            EntityConfig.SetupEntity<ApplicantsSkill, int>(builder);

            builder.Property(x => x.ApplicantId).IsRequired();
            builder.Property(x => x.SkillId).IsRequired();
            builder.HasOne(x => x.Skill).WithMany().HasForeignKey(x => x.SkillId).OnDelete(DeleteBehavior.Cascade);
        }

    }
    
}

