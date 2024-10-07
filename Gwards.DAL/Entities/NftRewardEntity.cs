using Gwards.DAL.Entities.Quests;

namespace Gwards.DAL.Entities;

public class NftRewardEntity : BaseEntity
{
    public uint Index { get; set; }
    public string Address { get; set; }
    public string MinterAddress { get; set; }
    
    public int QuestId { get; set; }
    public QuestBaseEntity Quest { get; set; }
}
