using Gward.Common.Enums;
using Gwards.DAL.Entities.Quests;
using Gwards.DAL.Enums;

namespace Gwards.DAL.Entities;

public class SubscribeQuestEntity : QuestBaseEntity
{
    public override QuestType Type => QuestType.Subscribe;
    public string TargetAccount { get; set; }
    public AccountType AccountType { get; set; }
}
