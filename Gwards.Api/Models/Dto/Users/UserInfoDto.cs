namespace Gwards.Api.Models.Dto.Users;

public class UserInfoDto
{
    public int Id { get; set; }
    public string Nickname { get; set; }
    public string AvatarPath { get; set; }
    public long TelegramId { get; set; }
    public string TonAddress { get; set; }
    
    public int Score { get; set; }
    public int GwardPointBalance { get; set; }
    
    public int ReferralsInvitedAmount { get; set; }
    public int ReferralsPointsRewardAmount { get; set; }
    public string ReferralLink { get; set; }
    
    public int DailyRewardAmount { get; set; }
    public int DailyRewardDaysClaimed { get; set; }
    public DateTime DailyRewardAvailableAt { get; set; }
    
    public bool HasLinkedSteamAccount { get; set; }
}
