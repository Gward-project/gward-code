using Newtonsoft.Json;

namespace Gwards.Api.Models.Dto.Transactions.TonApi;

public class RawMessageData
{
    [JsonProperty("hash")]
    public string BodyHash { get; set; }
    [JsonProperty("body")]
    public string Body { get; set; }
    [JsonProperty("decoded")]
    public MessageDataDecoded Decoded { get; set; }
}

public class MessageDataDecoded
{
    [JsonProperty("comment")]
    public string Comment { get; set; }
}