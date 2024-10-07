namespace Gwards.Api.Models.Dto.Nft;

public class NftRewardDto
{
    public string Address { get; set; }
    
    public string CollectionMetadataPath { get; set; }
    public string ItemMetadataPath { get; set; }
    public string ImagePath { get; set; }
    
    public DateTime CreatedAt { get; set; }
}