using Gward.Common.Enums;
using Gwards.DAL.Enums;

namespace Gwards.Api.Models.Dto.Quests;

public class QuestInfoDto
{
    public int Id { get; set; }
    public QuestType Type { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public RewardType RewardType { get; set; }
    public int? Reward { get; set; }
    public string RewardNftImagePath { get; set; }
    public QuestStatus Status { get; set; }
    public string ImagePath { get; set; }
}
