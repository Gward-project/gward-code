using Newtonsoft.Json;

namespace Gwards.Api.Models.Dto.Nft;

public class NftCollectionInfoDto
{
    [JsonProperty("address")]
    public string Address { get; set; }
    
    [JsonProperty("owner_address")]
    public string OwnerAddress { get; set; }
    
    [JsonProperty("next_item_index")]
    public string NextItemIndex { get; set; }
    
    [JsonProperty("last_transaction_lt")]
    public string LastTransactionLt { get; set; }
    
    [JsonProperty("data_hash")]
    public string DataHash { get; set; }
    
    [JsonProperty("code_hash")]
    public string CodeHash { get; set; }
}