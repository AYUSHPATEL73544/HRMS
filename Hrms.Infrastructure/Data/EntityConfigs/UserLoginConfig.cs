using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hrms.Infrastructure.Data.EntityConfigs
{
    internal class UserLoginConfig : IEntityTypeConfiguration<IdentityUserLogin<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<int>> builder)
        {
            builder.ToTable("UserLogins").HasKey(x => new { x.LoginProvider, x.ProviderKey });

            builder.Property(x => x.LoginProvider).HasMaxLength(100);
            builder.Property(x => x.ProviderKey).HasMaxLength(100);
            builder.Property(x => x.ProviderDisplayName).IsRequired().HasMaxLength(250);
            builder.Property(x => x.UserId).IsRequired();
        }
    }
}
