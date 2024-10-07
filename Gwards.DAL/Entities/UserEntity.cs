namespace Gwards.DAL.Entities;

public class UserEntity : BaseEntity
{
    public string Nickname { get; set; }
    public string AvatarPath { get; set; }
    public long TelegramId { get; set; }
    public long? ChatId { get; set; }
    
    public string TonAddress { get; set; }
    
    public int Score { get; set; }
    public int GwardPointBalance { get; set; }
    
    public string ReferralCode { get; set; }
    public int ReferralsInvitedAmount { get; set; }
    public int ReferralsPointsRewardAmount { get; set; }
    public bool IsReferralCodeApplied { get; set; }
    
    public DailyRewardEntity DailyReward { get; set; }
    
    public virtual ICollection<ExternalAccountEntity> ExternalAccounts { get; set; }
    public virtual ICollection<UserQuestEntity> UserQuests { get; set; }
    public virtual ICollection<PaymentEntity> Payments { get; set; }
    
    public DateTime AvatarFetchedAt { get; set; }
}
