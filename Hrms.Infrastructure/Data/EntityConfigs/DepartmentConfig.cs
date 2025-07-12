using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Hrms.Core.Entities;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");

            EntityConfig.SetupEntityWithStatusTracking<Department, int>(builder);

            builder.Property(x => x.Description).HasMaxLength(500);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Code).IsRequired().HasMaxLength(2);

            builder.HasMany(x => x.Employees).WithOne().HasForeignKey(x => x.DepartmentId); 
            builder.HasMany(x => x.WorkHistories).WithOne().HasForeignKey(x => x.DepartmentId); 


        }
    }
}
