using Newtonsoft.Json;

namespace Gwards.Api.Models.Dto.Nft;

public class NftMetadataDto
{
    [JsonProperty("name")]
    public string Name { get; set; }
    [JsonProperty("description")]
    public string Description { get; set; }
    [JsonProperty("image")]
    public string Image { get; set; }
    [JsonProperty("attributes")]
    public NftMetadataAttribute[] Attributes { get; set; } = Array.Empty<NftMetadataAttribute>();
}
