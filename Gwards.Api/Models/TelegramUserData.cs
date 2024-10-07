using Newtonsoft.Json;

namespace Gwards.Api.Models;

public class TelegramUserData
{
    [JsonProperty("id")]
    public long Id { get; set; }
    
    [JsonProperty("first_name")]
    public string FirstName { get; set; }
    
    [JsonProperty("last_name")]
    public string LastName { get; set; }
    
    [JsonProperty("username")]
    public string Username { get; set; }
    
    [JsonProperty("photo_url")]
    public string PhotoUrl { get; set; }
}