using Gwards.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gwards.DAL.Configurations;

internal class UserConfiguration : BaseEntityConfiguration<UserEntity>
{
    public override void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        base.Configure(builder);
        
        builder
            .HasIndex(x => x.TonAddress)
            .IsUnique();
    }
}
