using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class Documentconfig : IEntityTypeConfiguration<Document>
    {

        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Documents");

            EntityConfig.SetupEntityBase<Document, int>(builder);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.IdentificationId).IsRequired();
            builder.Property(x => x.Key).IsRequired();
            builder.Property(x => x.DocumentType).IsRequired();

        }
    }
 }

