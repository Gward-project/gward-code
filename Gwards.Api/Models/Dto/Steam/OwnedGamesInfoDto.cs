using Steam.Models.SteamCommunity;

namespace Gwards.Api.Models.Dto.Steam;

public class OwnedGamesInfoDto
{
    public uint GameCount { get; set; }
    public int TotalPlayTime { get; set; }

    public IReadOnlyCollection<OwnedGameModel> OwnedGames { get; set; }
}