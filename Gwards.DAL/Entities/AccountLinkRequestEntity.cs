namespace Gwards.DAL.Entities;

public class AccountLinkRequestEntity : BaseEntity
{
    public int UserId { get; set; }
    public string Type { get; set; } //todo: enum
    public string Token { get; set; }

    public virtual UserEntity User { get; set; }
}
