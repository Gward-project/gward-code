using Gwards.Api.Services;
using Gwards.DAL;
using Gwards.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Steam.Models.SteamCommunity;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;
using System.Text.RegularExpressions;
using System.Web;
using Gwards.Api.Models.Dto.Steam;

namespace Gwards.Api.Controllers;

[Authorize]
public class SteamController : BaseController
{
    private readonly SteamService _steamService;
    private readonly GwardsContext _gwardsContext;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly SteamWebInterfaceFactory _steamFactory;

    public SteamController(
        SteamService steamService,
        GwardsContext gwardsContext,
        IHttpClientFactory httpClientFactory,
        SteamWebInterfaceFactory steamWebInterfaceFactory
    )
    {
        _steamService = steamService;
        _gwardsContext = gwardsContext;
        _httpClientFactory = httpClientFactory;
        _steamFactory = steamWebInterfaceFactory;
    }

    [HttpGet("signin-by-steam")]
    public async Task<string> SignIn([FromQuery] string redirectUrl)
    {
        var userId = ExtractUserId();
        var token = Guid.NewGuid().ToString();

        var linkRequest = new AccountLinkRequestEntity()
        {
            Token = token,
            UserId = userId,
            Type = "steam",
            CreatedAt = DateTime.UtcNow,
        };

        _gwardsContext.AccountLinkRequest.Add(linkRequest);
        await _gwardsContext.SaveChangesAsync();

        return GetRedirectUrl(token, redirectUrl);
    }

    [HttpGet("validate-steam-signin")]
    public async Task ValidateSteamSignin([FromQuery] string redirectUrl)
    {
        var isSignInValid = await _steamService.ValidateSignin(redirectUrl);
        if (!isSignInValid)
        {
            throw new InvalidOperationException();
        }

        var stateMatch = Regex.Match(redirectUrl, "state=(.*?)\\&open");
        if (!stateMatch.Success)
        {
            throw new InvalidOperationException();
        }

        var linkRequestToken = stateMatch.Groups[1].Value;
        var linkRequest = await _gwardsContext.AccountLinkRequest.FirstOrDefaultAsync(x => x.Token == linkRequestToken);
        if (linkRequest == null)
        {
            throw new InvalidOperationException();
        }

        redirectUrl = HttpUtility.UrlDecode(redirectUrl);
        var steamUserIdMatch = Regex.Match(redirectUrl, "openid\\.identity=https:\\/\\/steamcommunity\\.com\\/openid\\/id\\/(\\d+)&");
        if (!steamUserIdMatch.Success)
        {
            throw new InvalidOperationException();
        }

        await _steamService.LinkSteamAccount(steamUserIdMatch.Groups[1].Value, linkRequest.UserId);
        _gwardsContext.AccountLinkRequest.Remove(linkRequest);
        await _gwardsContext.SaveChangesAsync();
    }

    [HttpGet("owned-games")]
    [Authorize]
    public async Task<OwnedGamesInfoDto> GetOwnedGames()
    {
        var userId = ExtractUserId();
        var steamAccount = await _gwardsContext.ExternalAccounts.FirstOrDefaultAsync(x => x.UserId == userId && x.Type == "steam");

        var httpClient = _httpClientFactory.CreateClient();
        var response = await _steamFactory
            .CreateSteamWebInterface<PlayerService>(httpClient)
            .GetOwnedGamesAsync(ulong.Parse(steamAccount.AccountId), includeFreeGames: true);
        var totalPlayTime = response.Data
            .OwnedGames
            .Sum(x => (int)x.PlaytimeForever.TotalHours);
        
        return new OwnedGamesInfoDto
        {
            GameCount = response.Data.GameCount,
            TotalPlayTime = totalPlayTime,
            OwnedGames = response.Data.OwnedGames,
        };
    }

    [HttpGet("player-summary")]
    [Authorize]
    public async Task<PlayerSummaryModel> GetPlayerSummary()
    {
        var userId = ExtractUserId();
        var steamAccount = await _gwardsContext
            .ExternalAccounts
            .FirstOrDefaultAsync(x => x.UserId == userId && x.Type == "steam");

        var httpClient = _httpClientFactory.CreateClient();
        var steamWebResponse = await _steamFactory
            .CreateSteamWebInterface<SteamUser>(httpClient)
            .GetPlayerSummaryAsync(ulong.Parse(steamAccount.AccountId));

        return steamWebResponse.Data;
    }

    [HttpGet("app/{appId}")]
    [Authorize]
    public async Task<IResult> GetApplicationData(uint appId)
    {
        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.GetAsync("https://store.steampowered.com/api/appdetails?appids=" + appId);

        return Results.Content(await response.Content.ReadAsStringAsync(), "application/json");
    }

    private string GetRedirectUrl(string token, string redirectUrl)
    {
        var url = new Uri(redirectUrl);
        return "https://steamcommunity.com/openid/login?" +
            "openid.claimed_id=http://specs.openid.net/auth/2.0/identifier_select" +
            "&openid.identity=http://specs.openid.net/auth/2.0/identifier_select" +
            "&openid.mode=checkid_setup" +
            "&openid.ns=http://specs.openid.net/auth/2.0" +
            "&openid.realm=" + url.Scheme + "://" + url.Host +
            "&openid.return_to="+redirectUrl+"?state=" + token +
            "&openid.ns.ax=http://openid.net/srv/ax/1.0" +
            "&openid.ax.mode=fetch_request" +
            "&openid.ax.type.email=http://axschema.org/contact/email" +
            "&openid.ax.type.name=http://axschema.org/namePerson" +
            "&openid.ax.type.first=http://axschema.org/namePerson/first" +
            "&openid.ax.type.last=http://axschema.org/namePerson/last" +
            "&openid.ax.type.email2=http://schema.openid.net/contact/email" +
            "&openid.ax.type.name2=http://schema.openid.net/namePerson" +
            "&openid.ax.type.first2=http://schema.openid.net/namePerson/first" +
            "&openid.ax.type.last2=http://schema.openid.net/namePerson/last" +
            "&openid.ax.required=email,name,first,last,email2,name2,first2,last";
    }
}
