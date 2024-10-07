using Gwards.Api.Models.Dto.Transactions.TonApi;

namespace Gwards.Api.Models.Dto.Transactions.Processing;

public class ProcessedTransactionInfoDto
{
    public TransactionDto TransactionDto { get; set; }
    public object Metadata { get; set; }
}