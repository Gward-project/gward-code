namespace Gwards.DAL.Entities;

public class TransactionEntity : BaseEntity
{
    public string Hash { get; set; }
    
    public string Source { get; set; }
    public string Destination { get; set; }
    
    public string Value { get; set; }
    public string Comment { get; set; }
}
