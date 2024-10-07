using Newtonsoft.Json;

namespace Gwards.Api.Models.Dto.Transactions.TonApi;

public class RootTransactionsDto
{
    [JsonProperty("transactions")]
    public TransactionDto[] Transactions { get; set; }
}