using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    public class ReimbursementConfig: IEntityTypeConfiguration<Reimbursement>
    {
       public void Configure(EntityTypeBuilder<Reimbursement> builder)
        {
            builder.ToTable("Reimbursements");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Description).HasMaxLength(500).IsRequired();
            builder.Property(e => e.Amount).IsRequired();
            builder.Property(e => e.Date).IsRequired();
            builder.Property(e => e.PaymentDate);
            builder.Property(e => e.Remark).HasMaxLength(500);

            builder.HasOne(e => e.Employee).WithMany().HasForeignKey(e => e.EmployeeId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
