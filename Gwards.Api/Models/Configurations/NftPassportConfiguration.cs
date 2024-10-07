namespace Gwards.Api.Models.Configurations;

public class NftPassportConfiguration
{
    public string CollectionAddress { get; set; }
    public string BaseMetadataPath { get; set; }
    public int MinScoreRequired { get; set; }
}