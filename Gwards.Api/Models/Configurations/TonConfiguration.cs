namespace Gwards.Api.Models.Configurations;

public class TonConfiguration
{
    public string ApiUrl { get; set; }
    public string ApiKey { get; set; }
    public int WorkChain { get; set; }
    public string[] MasterWalletMnemonic { get; set; }
}