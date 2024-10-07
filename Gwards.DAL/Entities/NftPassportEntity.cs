namespace Gwards.DAL.Entities;

public class NftPassportEntity : BaseEntity
{
    public uint Index { get; set; }
    public string Address { get; set; }
    public string MinterAddress { get; set; }
    public string MetadataPath { get; set; }
    public string ImagePath { get; set; }
    
    public int Score { get; set; }
    
    
    public int PaymentId { get; set; }
    public PaymentEntity Payment { get; set; }
}