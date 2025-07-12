using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class FamilyConfig : IEntityTypeConfiguration<Family>
    {
        public void Configure(EntityTypeBuilder<Family> builder)
        {
            builder.ToTable("Families");

            EntityConfig.SetupEntityBase<Family, int>(builder);

            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(240);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(240);
            builder.Property(x => x.Email);
            builder.Property(x => x.Phone);
            builder.Property(x => x.DateOfBirth);
            builder.Property(x => x.RelationshipId).IsRequired();
            builder.Property(x => x.EmployeeId);
            builder.HasOne(x => x.Relationship).WithMany().HasForeignKey(x => x.RelationshipId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
