using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {

            builder.ToTable("Employees");

            EntityConfig.SetupEntityBase<Employee, int>(builder);

            builder.Property(x => x.Code).HasMaxLength(40);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.CompanyId).IsRequired();
            builder.Property(x => x.DesignationId);
            builder.Property(x => x.DepartmentId);
            builder.Property(x => x.ManagerId);
            builder.Property(x => x.FirstName).IsRequired();
            builder.Property(x => x.LastName).IsRequired();
            builder.Property(x => x.Gender).IsRequired();
            builder.Property(x => x.Phone).IsRequired();
            builder.Property(x => x.Email).IsRequired();
            builder.Property(x => x.BloodGroup);
            builder.Property(x => x.MaritalStatus).IsRequired();
            builder.Property(x => x.AlternatePhone);
            builder.Property(x => x.AlternateEmail);
            builder.Property(x => x.DateOfBirth);
            builder.Property(x => x.DateOfJoining).IsRequired();
            builder.Property(x => x.DateOfLeaving);
            builder.Property(x => x.ExitType);
            builder.Property(x => x.ProbationPeriod).IsRequired();
            builder.Property(x => x.NoticePeriod);
            builder.Property(x => x.Note).HasMaxLength(250);
            builder.Property(x => x.ExitDate);
            builder.Property(x => x.EmployeeType).IsRequired();


            builder.HasMany(x => x.Attendances).WithOne().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Leaves).WithOne().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.LeaveLogs).WithOne().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Educations).WithOne().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.Families).WithOne().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(x => x.WorkHistories).WithOne().HasForeignKey(x => x.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            

            

        }
    }
}
