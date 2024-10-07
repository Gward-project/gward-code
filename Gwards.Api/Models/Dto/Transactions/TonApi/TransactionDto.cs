using Newtonsoft.Json;

namespace Gwards.Api.Models.Dto.Transactions.TonApi;

public class TransactionDto
{
    [JsonProperty("account")]
    public string Account { get; set; }
    
    [JsonProperty("now")]
    public long Now { get; set; }
    
    [JsonProperty("lt")]
    public ulong Lt { get; set; }
    
    [JsonProperty("hash")]
    public string Hash { get; set; }
    
    [JsonProperty("total_fees")]
    public string Fee { get; set; }
    
    [JsonProperty("prev_trans_hash")]
    public string PrevTransHash { get; set; }
    
    [JsonProperty("prev_trans_lt")]
    public string PrevTransLt { get; set; }
    
    [JsonProperty("description")]
    public TransactionDescription Description { get; set; }
    
    [JsonProperty("in_msg")]
    public RawMessage InMsg { get; set; }
    
    [JsonProperty("out_msgs")]
    public RawMessage[] OutMsgs { get; set; }
}