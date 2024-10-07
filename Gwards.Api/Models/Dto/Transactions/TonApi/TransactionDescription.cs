using Newtonsoft.Json;

namespace Gwards.Api.Models.Dto.Transactions.TonApi;

public class TransactionDescription
{
    [JsonProperty("type")]
    public string Type { get; set; }
    
    [JsonProperty("aborted")]
    public bool Aborted { get; set; }
    
    [JsonProperty("destroyed")]
    public bool Destroyed { get; set; }
    
    [JsonProperty("credit_first")]
    public bool CreditFirst { get; set; }
    
    [JsonProperty("compute_ph")]
    public ComputePhaseDescription ComputePhase { get; set; }
    
    [JsonProperty("action")]
    public ActionPhaseDescription ActionPhase { get; set; }
}