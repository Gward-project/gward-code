using Gwards.DAL.Enums;

namespace Gwards.DAL.Entities.Quests;

public class MintQuestEntity : QuestBaseEntity
{
    public override QuestType Type => QuestType.Mint;
    public int? NftQuestId { get; set; }
}
