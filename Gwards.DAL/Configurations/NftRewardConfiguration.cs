using Gwards.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gwards.DAL.Configurations;

internal class NftRewardConfiguration : BaseEntityConfiguration<NftRewardEntity>
{
    public override void Configure(EntityTypeBuilder<NftRewardEntity> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(x => x.Quest)
            .WithMany(x => x.NftRewards)
            .OnDelete(DeleteBehavior.Cascade);
    }
}