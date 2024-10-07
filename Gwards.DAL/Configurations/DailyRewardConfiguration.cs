using Gwards.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gwards.DAL.Configurations;

internal class DailyRewardConfiguration : BaseEntityConfiguration<DailyRewardEntity>
{
    public override void Configure(EntityTypeBuilder<DailyRewardEntity> builder)
    {
        base.Configure(builder);

        builder
            .HasOne(x => x.User)
            .WithOne(x => x.DailyReward)
            .OnDelete(DeleteBehavior.Cascade);
    }
}