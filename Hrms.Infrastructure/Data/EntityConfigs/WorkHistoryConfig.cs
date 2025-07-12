using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class WorkHistoryConfig : IEntityTypeConfiguration<WorkHistory>
    {
        public void Configure(EntityTypeBuilder<WorkHistory> builder)
        {
            builder.ToTable("WorkHistories");

            EntityConfig.SetupEntityWithStatusTracking<WorkHistory, int>(builder);

            builder.Property(x => x.EmployeeId).IsRequired();
            builder.Property(x => x.DepartmentId).IsRequired();
            builder.Property(x => x.DesignationId).IsRequired();
            builder.Property(x => x.From).IsRequired();
            builder.Property(x => x.To);
        }
    }
}
