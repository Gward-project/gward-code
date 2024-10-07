namespace Gwards.DAL.Entities;

public class ExternalAccountEntity : BaseEntity
{
    public int UserId { get; set; }
    public string Type { get; set; }
    public string AccountId { get; set; }
    public string AccountName { get; set; }

    public virtual UserEntity User { get; set; }
}
