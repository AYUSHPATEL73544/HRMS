using Hrms.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    public static class EntityConfig
    {
        public static void SetupEntity<T, U>(EntityTypeBuilder<T> builder, bool valueGeneratedOnAdd = true) where T : Entity<U>
        {
            builder.HasKey(x => x.Id);

            if (valueGeneratedOnAdd)
            {
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            }
            else
            {
                builder.Property(x => x.Id).ValueGeneratedNever();
            }
        }

        public static void SetupEntityWithStatusTracking<T, U>(EntityTypeBuilder<T> builder, bool valueGeneratedOnAdd = true) where T : EntityWithStatusTracking<U>
        {
            builder.HasKey(x => x.Id);

            if (valueGeneratedOnAdd)
            {
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            }
            else
            {
                builder.Property(x => x.Id).ValueGeneratedNever();
            }

            builder.Property(x => x.Status).IsRequired();
        }

        public static void SetupEntityWithUserTracking<T, U>(EntityTypeBuilder<T> builder, bool valueGeneratedOnAdd = true) where T : EntityWithUserTracking<U>
        {
            builder.HasKey(x => x.Id);

            if (valueGeneratedOnAdd)
            {
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            }
            else
            {
                builder.Property(x => x.Id).ValueGeneratedNever();
            }

            builder.Property(x => x.CreatedById).IsRequired();
            builder.Property(x => x.UpdatedById);
        }

        public static void SetupEntityWithTimeTracking<T, U>(EntityTypeBuilder<T> builder, bool valueGeneratedOnAdd = true) where T : EntityWithTimeTracking<U>
        {
            builder.HasKey(x => x.Id);

            if (valueGeneratedOnAdd)
            {
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            }
            else
            {
                builder.Property(x => x.Id).ValueGeneratedNever();
            }

            builder.Property(x => x.CreatedOn).IsRequired().HasColumnType("datetime");
            builder.Property(x => x.UpdatedOn).HasColumnType("datetime");
            builder.Property(x => x.EffectiveFrom).IsRequired().HasColumnType("datetime");
            builder.Property(x => x.EffectiveTo).HasColumnType("datetime");
        }

        public static void SetupEntityBase<T, U>(EntityTypeBuilder<T> builder, bool valueGeneratedOnAdd = true) where T : EntityBase<U>
        {
            builder.HasKey(x => x.Id);

            if (valueGeneratedOnAdd)
            {
                builder.Property(x => x.Id).ValueGeneratedOnAdd();
            }
            else
            {
                builder.Property(x => x.Id).ValueGeneratedNever();
            }

            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.CreatedById).IsRequired();
            builder.Property(x => x.UpdatedById);

            builder.Property(x => x.CreatedOn).IsRequired().HasColumnType("datetime");
            builder.Property(x => x.UpdatedOn).HasColumnType("datetime");
            builder.Property(x => x.EffectiveFrom).IsRequired().HasColumnType("date");
            builder.Property(x => x.EffectiveTo).HasColumnType("date");
        }
    }
}
