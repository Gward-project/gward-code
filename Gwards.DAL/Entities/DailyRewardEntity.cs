namespace Gwards.DAL.Entities;

public class DailyRewardEntity : BaseEntity
{
    public int Day { get; set; }
    public DateTime LastRewardClaimedAt { get; set; }
    
    public int UserId { get; set; }
    public UserEntity User { get; set; }
}