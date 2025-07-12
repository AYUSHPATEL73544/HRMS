using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class LeaveConfig : IEntityTypeConfiguration<Leave>
    {
        public void Configure(EntityTypeBuilder<Leave> builder)
        {
            builder.ToTable("Leaves").HasKey(x => x.Id);


            EntityConfig.SetupEntityBase<Leave, int>(builder);

            builder.Property(x => x.Id);
            builder.Property(x => x.EmployeeId).IsRequired();
            builder.Property(x => x.RuleId).IsRequired();
            builder.Property(x => x.Total).HasColumnType("decimal(5,2)").IsRequired();
            builder.Property(x => x.Credited).HasColumnType("decimal(5,2)").IsRequired();
            builder.Property(x => x.Available).HasColumnType("decimal(5,2)").IsRequired();
            builder.Property(x => x.Applied).HasColumnType("decimal(5,2)").IsRequired();
        }
    }
}
