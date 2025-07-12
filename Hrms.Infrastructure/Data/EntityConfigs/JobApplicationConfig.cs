using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class JobApplicationConfig : IEntityTypeConfiguration<JobApplication>
    {

        public void Configure(EntityTypeBuilder<JobApplication> builder)
        {
            builder.ToTable("JobApplications");

            EntityConfig.SetupEntityBase<JobApplication, int>(builder);

            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Phone).IsRequired().HasMaxLength(20);
            builder.Property(x => x.QualificationTypeId).IsRequired();
            builder.Property(x => x.CourseTypeId).IsRequired();
            builder.Property(x => x.CourseName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Stream).IsRequired().HasMaxLength(100);
            builder.Property(x => x.PassingYear).IsRequired();
            builder.Property(x => x.Pursuing).IsRequired();
            builder.Property(x => x.Remark).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.Shortlisted).IsRequired();
            builder.Property(x => x.Hired).IsRequired();
            builder.Property(x => x.ShortlistedDate).IsRequired(false);
            builder.Property(x => x.MarketingChannel).HasMaxLength(100);
            builder.Property(x => x.Gender).IsRequired();

            builder.HasMany(x => x.ApplicantsSkills).WithOne().HasForeignKey(x => x.ApplicantId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Interviews).WithOne().HasForeignKey(x => x.CandidateId).OnDelete(DeleteBehavior.Cascade);

        }
    }
}
