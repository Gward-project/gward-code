namespace Gwards.Api.Models.Configurations;

public class JwtConfiguration
{
    public string Issuer { get; set; }
    public string Secret { get; set; }
    public string Audience { get; set; }
    public int AccessTokenLifetimeMin { get; set; }
    public int RefreshTokenLifetimeMin { get; set; }
}
