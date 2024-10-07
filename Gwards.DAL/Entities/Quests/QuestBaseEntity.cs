using Gward.Common.Enums;
using Gwards.DAL.Enums;

namespace Gwards.DAL.Entities.Quests;

public abstract class QuestBaseEntity : BaseEntity
{
    public virtual QuestType Type { get; protected set; }
    public string ImagePath { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsDefault { get; set; }
    
    public RewardType RewardType { get; set; }
    public int? Reward { get; set; }
    
    public string NftCollectionAddress { get; set; }
    public string NftImagePath { get; set; }
    public string NftMetadataBasePath { get; set; }

    public ICollection<UserQuestEntity> UserQuests { get; set; }
    public ICollection<NftRewardEntity> NftRewards { get; set; }
}
