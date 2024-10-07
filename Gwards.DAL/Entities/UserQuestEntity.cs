using Gwards.DAL.Entities.Quests;
using Gwards.DAL.Enums;

namespace Gwards.DAL.Entities;

public class UserQuestEntity : BaseEntity
{
    public int UserId { get; set; }
    public int QuestId { get; set; }
    public QuestStatus Status { get; set; }
    public DateTime? ClaimedAt { get; set; }

    public virtual UserEntity User { get; set; }
    public virtual QuestBaseEntity Quest { get; set; }
}