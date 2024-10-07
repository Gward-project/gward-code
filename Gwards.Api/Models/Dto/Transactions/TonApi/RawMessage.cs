using Newtonsoft.Json;

namespace Gwards.Api.Models.Dto.Transactions.TonApi;

public class RawMessage
{
    [JsonProperty("hash")]
    public string Hash { get; set; }
    [JsonProperty("source")]
    public string Source { get; set; }
    [JsonProperty("destination")]
    public string Destination { get; set; }
    [JsonProperty("value")]
    public string Value { get; set; }
    [JsonProperty("opcode")]
    public string OpCode { get; set; }
    [JsonProperty("fwd_fee")]
    public string FwdFee { get; set; }
    [JsonProperty("ihr_fee")]
    public string IhrFee { get; set; }
    [JsonProperty("created_lt")]
    public ulong CreatedLt { get; set; }
    [JsonProperty("message_content")]
    public RawMessageData MsgData { get; set; }
}