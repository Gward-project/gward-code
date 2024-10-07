using Gwards.DAL.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gwards.DAL.Configurations;

internal class UserQuestConfiguration : BaseEntityConfiguration<UserQuestEntity>
{
    public override void Configure(EntityTypeBuilder<UserQuestEntity> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.User).WithMany(x => x.UserQuests).HasForeignKey(x => x.UserId);
        builder.HasOne(x => x.Quest).WithMany(x => x.UserQuests).HasForeignKey(x => x.QuestId);

        builder.HasIndex(x => new { x.QuestId, x.UserId });
    }
}
