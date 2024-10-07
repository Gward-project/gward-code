using Gwards.DAL.Enums;

namespace Gwards.DAL.Entities.Quests;

public class WalletQuestEntity : QuestBaseEntity
{
    public override QuestType Type => QuestType.Wallet;
}
