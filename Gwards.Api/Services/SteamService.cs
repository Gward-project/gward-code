using Gward.Common.Exceptions;
using Gwards.Api.Constants;
using Gwards.Api.Models.Dto.Scoring;
using Gwards.DAL;
using Gwards.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Steam.Models.SteamCommunity;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;

namespace Gwards.Api.Services;

public class SteamService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly SteamWebInterfaceFactory _steamFactory;
    private readonly ScoringService _scoringService;
    private readonly GwardsContext _gwardsContext;

    public SteamService(
        IHttpClientFactory httpClientFactory,
        SteamWebInterfaceFactory steamFactory,
        ScoringService scoringService,
        GwardsContext gwardsContext
    )
    {
        _httpClientFactory = httpClientFactory;
        _steamFactory = steamFactory;
        _scoringService = scoringService;
        _gwardsContext = gwardsContext;
    }

    public async Task<ScoringDataDto> GetScoringData(ulong steamAccountId)
    {
        var httpClient = _httpClientFactory.CreateClient();

        var profileData = new PlayerSummaryModel();
        var friendsData = new List<FriendModel>();
        var gamesData = new OwnedGamesResultModel();

        try
        {
            var profileResponse = await _steamFactory
                .CreateSteamWebInterface<SteamUser>(httpClient)
                .GetPlayerSummaryAsync(steamAccountId);
            profileData = profileResponse.Data;
        }
        catch
        {
            // ignored
        }

        try
        {
            var friendsResponse = await _steamFactory
                .CreateSteamWebInterface<SteamUser>(httpClient)
                .GetFriendsListAsync(steamAccountId);
            friendsData = friendsResponse.Data.ToList();
        }
        catch
        {
            // ignored
        }

        try
        {
            var gamesResponse = await _steamFactory
                .CreateSteamWebInterface<PlayerService>(httpClient)
                .GetOwnedGamesAsync(steamAccountId, includeFreeGames: true);
            
            gamesData = gamesResponse.Data;
        }
        catch
        {
            // ignored   
        }
        
        return new ScoringDataDto
        {
            AccountLifeTimeDays = (DateTime.UtcNow - profileData.AccountCreatedDate).TotalDays,
            FriendsAmount = friendsData.Count,
            GamesAmount = gamesData.GameCount,
            HasAvatar = !string.IsNullOrEmpty(profileData.AvatarUrl),
            PlayTimeTotalHours = gamesData.OwnedGames.Sum(x => x.PlaytimeForever.TotalHours),
            PlayTimeTwoWeeksHours = gamesData.OwnedGames.Sum(x => x.PlaytimeLastTwoWeeks?.TotalHours) ?? 0
        };
    }
    
    public async Task<bool> ValidateSignin(string redirectUrl)
    {
        var urlParams = redirectUrl.Substring(redirectUrl.IndexOf('?')).Replace("id_res", "check_authentication");
        var validationUrl = SteamConstants.AuthBaseUrl + urlParams;

        var httpClient = new HttpClient(); //todo: http factory
        var validateResponse = await httpClient.GetAsync(validationUrl);
          
        var response = await validateResponse.Content.ReadAsStringAsync();

        return response.Contains("is_valid:true");
    }
    
    public async Task LinkSteamAccount(string externalUserId, int userId)
    {
        var user = await _gwardsContext
            .Users
            .Include(x => x.ExternalAccounts)
            .FirstOrDefaultAsync(x => x.Id == userId);
            
        if (user == null) 
        {
            throw new GwardException("User not found");
        }

        if (user.ExternalAccounts.Any(x => x.Type == "steam"))
        {
            throw new GwardException("Another account already linked");
        }

        var isSteamAccountAlreadyLinked = await _gwardsContext.ExternalAccounts.AnyAsync(x => x.Type == "steam" && x.AccountId == externalUserId);
        if (isSteamAccountAlreadyLinked)
        {
            throw new GwardException("Current steam account already linked");
        }

        var steamAccountId = ulong.Parse(externalUserId);
        var httpClient = _httpClientFactory.CreateClient();
        var gamesResponse = await _steamFactory
            .CreateSteamWebInterface<PlayerService>(httpClient)
            .GetOwnedGamesAsync(steamAccountId, includeFreeGames: true);

        user.GwardPointBalance += (int)gamesResponse.Data.OwnedGames.Sum(x => x.PlaytimeForever.TotalHours);
        user.Score = await _scoringService.CalculateScore(steamAccountId);
        
        user.ExternalAccounts.Add(new ExternalAccountEntity
        {
            AccountId = externalUserId,
            AccountName = "111",
            Type = "steam",
            User = user,
            CreatedAt = DateTime.UtcNow,
        });
           
        await _gwardsContext.SaveChangesAsync();
    }
}