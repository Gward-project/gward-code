namespace Gwards.Api.Models.Dto.Nft;

public class NftPassportInfoDto
{
    public string Address { get; set; }
    public string MetadataPath { get; set; }
    public string ImagePath { get; set; }
    
    public bool IsEnoughScoreForMint { get; set; }
    public bool IsPassportUpgradable { get; set; }
}