using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class CompanyConfig : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable("Companies").HasKey(x => x.Id);

            EntityConfig.SetupEntityBase<Company, int>(builder);

            builder.Property(x => x.Id).HasMaxLength(40);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.RegisteredName).IsRequired().HasMaxLength(250);
            builder.Property(x => x.BrandName).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Email).IsRequired().HasMaxLength(250);
            builder.Property(x => x.Phone).IsRequired().HasMaxLength(10);
            builder.Property(x => x.WebsiteUrl).HasMaxLength(250);
            builder.Property(x => x.LinkedInUrl).HasMaxLength(250);
            builder.Property(x => x.FacebookUrl).HasMaxLength(250);
            builder.Property(x => x.TwitterUrl).HasMaxLength(250);

            builder.HasMany(x => x.AttendanceRules).WithOne().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade); 
            builder.HasMany(x => x.Employees).WithOne().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.LeaveRules).WithOne().HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Cascade);
            

        }
    }
}
