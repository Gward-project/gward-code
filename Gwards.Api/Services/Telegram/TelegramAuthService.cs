using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Gwards.Api.Models;
using Gwards.Api.Utils;
using Gwards.DAL;
using Gwards.DAL.Entities;
using Gwards.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Gwards.Api.Services.Telegram;

public class TelegramAuthService
{
    private readonly TelegramBotConfig _telegramBotConfig;
    
    private readonly AuthUtils _authUtils;
    private readonly GwardsContext _gwardsContext;

    public TelegramAuthService(
        TelegramBotConfig telegramBotConfig,
        AuthUtils authUtils,
        GwardsContext gwardsContext
    )
    {
        _telegramBotConfig = telegramBotConfig;
        _authUtils = authUtils;
        _gwardsContext = gwardsContext;
    }

    public async Task<SignInResponse> SignIn(string initData)
    {
        var decodedUrl = HttpUtility.UrlDecode(initData);
        var requestData = HttpUtility.ParseQueryString(decodedUrl);
        
        var isValid = ValidateSingInRequest(requestData);
        if (!isValid)
        {
            throw new InvalidOperationException();
        }
        
        var userData = JsonConvert.DeserializeObject<TelegramUserData>(requestData["user"]);
        var user = await _gwardsContext.Users
            .Include(x => x.DailyReward)
            .FirstOrDefaultAsync(x => x.TelegramId == userData.Id);
        
        if (user == null)
        {
            user = new UserEntity { TelegramId = userData.Id, CreatedAt = DateTime.UtcNow };
            user.UserQuests = await GetDefaultQuests(user);
            _gwardsContext.Users.Add(user);
        }

        user.Nickname = userData.Username;
        
        await ClaimDailyReward(user);
        await _gwardsContext.SaveChangesAsync();
        
        return new SignInResponse
        {
            UserId = user.Id,
            AccessToken = _authUtils.GenerateAccessToken(user.Id),
        };
    }
    
    private async Task<List<UserQuestEntity>> GetDefaultQuests(UserEntity userEntity)
    {
        var defaultQuests = await _gwardsContext
            .Quests
            .Where(x => x.IsDefault)
            .ToListAsync();

        return defaultQuests.Select(x => new UserQuestEntity
        {
            User = userEntity,
            Quest = x,
            CreatedAt = DateTime.UtcNow,
            Status = QuestStatus.New,
        }).ToList();
    }

    private bool ValidateSingInRequest(NameValueCollection requestData)
    {
        var expectedHash = requestData["hash"];
        if (expectedHash == null)
            return false;

        var requestParams = requestData.AllKeys
            .Where(x => x != "hash")
            .OrderBy(x => x)
            .Select(x => $"{x}={requestData[x]}");

        var serializedParams = string.Join("\n", requestParams);

        var key = HMACSHA256.HashData(Encoding.UTF8.GetBytes("WebAppData"), Encoding.UTF8.GetBytes(_telegramBotConfig.BotApiKey));
        var hash = HMACSHA256.HashData(key, Encoding.UTF8.GetBytes(serializedParams));

        var actualHash = BitConverter.ToString(hash).Replace("-", "").ToLower();

        return actualHash == expectedHash;
    }
    
    private async Task ClaimDailyReward(UserEntity user)
    {
        if (user.DailyReward == null)
        {
            var dailyReward = new DailyRewardEntity
            {
                Day = 1,
                User = user,
                LastRewardClaimedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };

            user.GwardPointBalance += DailyRewardsUtil.GetReward(dailyReward.Day);
            await _gwardsContext.DailyRewards.AddAsync(dailyReward);
            return;
        }

        var claimAvailableAt = user.DailyReward.LastRewardClaimedAt + TimeSpan.FromDays(1);
        var isDayMissed = (DateTime.UtcNow - claimAvailableAt).Days >= 1;
        var isClaimAvailable = claimAvailableAt < DateTime.UtcNow;
        
        if (isDayMissed)
        {
            user.DailyReward.Day = 1;
            user.GwardPointBalance += DailyRewardsUtil.GetReward(user.DailyReward.Day);
        }

        if (!isDayMissed && isClaimAvailable)
        {
            user.DailyReward.Day = DailyRewardsUtil.GetNextDay(user.DailyReward.Day);
            user.GwardPointBalance += DailyRewardsUtil.GetReward(user.DailyReward.Day);
        }

        if (isDayMissed || isClaimAvailable)
        {
            user.DailyReward.LastRewardClaimedAt = DateTime.UtcNow;
        }
    }
}
