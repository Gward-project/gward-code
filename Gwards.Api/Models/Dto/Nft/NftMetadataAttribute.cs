using Newtonsoft.Json;

namespace Gwards.Api.Models.Dto.Nft;

public class NftMetadataAttribute
{
    [JsonProperty("trait_type")]
    public string TraitType { get; set; }
    
    [JsonProperty("value")]
    public string Value { get; set; }
}