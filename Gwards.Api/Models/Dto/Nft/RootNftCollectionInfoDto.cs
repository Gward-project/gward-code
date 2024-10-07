using Newtonsoft.Json;

namespace Gwards.Api.Models.Dto.Nft;

public class RootNftCollectionInfoDto
{
    [JsonProperty("nft_collections")]
    public NftCollectionInfoDto[] NftCollections { get; set; }
}