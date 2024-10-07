using Gward.Common.Enums;
using Gwards.DAL.Entities;
using Gwards.DAL.Entities.Quests;
using Gwards.DAL.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gwards.DAL.Configurations;

internal class QuestBaseConfiguration : BaseEntityConfiguration<QuestBaseEntity>
{
    public override void Configure(EntityTypeBuilder<QuestBaseEntity> builder)
    {
        base.Configure(builder);

        builder.HasDiscriminator(x => x.Type)
            .HasValue<SubscribeQuestEntity>(QuestType.Subscribe)
            .HasValue<WalletQuestEntity>(QuestType.Wallet)
            .HasValue<GameQuestEntity>(QuestType.GameAction)
            .HasValue<MintQuestEntity>(QuestType.Mint);

    }
}
