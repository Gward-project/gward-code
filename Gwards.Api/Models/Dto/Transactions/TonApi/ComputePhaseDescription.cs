using Newtonsoft.Json;

namespace Gwards.Api.Models.Dto.Transactions.TonApi;

public class ComputePhaseDescription
{
    [JsonProperty("skipped")]
    public bool Skipped { get; set; }
    
    [JsonProperty("success")]
    public bool Success { get; set; }
    
    [JsonProperty("msg_state_used")]
    public bool MsgStateUsed { get; set; }
    
    [JsonProperty("account_activated")]
    public bool AccountActivated { get; set; }
    
    [JsonProperty("exit_code")]
    public int ExitCode { get; set; }
}