using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class TeamConfig: IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder.ToTable("Teams");
            EntityConfig.SetupEntityBase<Team, int>(builder);

            builder.Property(x => x.EmployeeId);
            builder.Property(x => x.Type);
            builder.Property(x => x.ManagerId);
        }
    }
}
