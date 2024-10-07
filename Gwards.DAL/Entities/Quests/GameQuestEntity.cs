using Gwards.DAL.Enums;

namespace Gwards.DAL.Entities.Quests;

public class GameQuestEntity : QuestBaseEntity
{
    public override QuestType Type => QuestType.GameAction;
    public string Platform { get; set; }
    public long ApplicationId { get; set; }
}
