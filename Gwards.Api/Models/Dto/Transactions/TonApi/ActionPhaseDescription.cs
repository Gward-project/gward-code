using Newtonsoft.Json;

namespace Gwards.Api.Models.Dto.Transactions.TonApi;

public class ActionPhaseDescription
{
    [JsonProperty("success")]
    public bool Success { get; set; }
    
    [JsonProperty("valid")]
    public bool Valid { get; set; }
    
    [JsonProperty("no_funds")]
    public bool NoFunds { get; set; }
    
    [JsonProperty("result_code")]
    public bool ResultCode { get; set; }
    
    [JsonProperty("tot_actions")]
    public bool TotActions { get; set; }
    
    [JsonProperty("msgs_created")]
    public bool MsgsCreated { get; set; }
}