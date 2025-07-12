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
    internal class NoteConfig : IEntityTypeConfiguration<Note>
    {
        public void Configure(EntityTypeBuilder<Note> builder)
        {
            builder.ToTable("Notes");
            EntityConfig.SetupEntityBase<Note, int>(builder);

            builder.Property(x => x.EmployeeId).IsRequired();
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500); ;

        }
    }
}
