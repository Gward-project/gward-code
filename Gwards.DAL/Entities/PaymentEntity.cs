using Gward.Common.Enums;

namespace Gwards.DAL.Entities;

public class PaymentEntity : BaseEntity
{
    public string Amount { get; set; }
    public string Comment { get; set; }
    public PostPaymentMethod PostPaymentMethod { get; set; }
    
    public int UserId { get; set; }
    public UserEntity User { get; set; }
    
    public int? TransactionId { get; set; }
    public TransactionEntity Transaction { get; set; }
}