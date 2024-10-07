using System.Text;
using System.Web;
using Gward.Common.Exceptions;
using Gwards.Api.Models;
using Gwards.Api.Models.Configurations;
using Gwards.DAL;
using Gwards.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Gwards.Api.Services.Telegram;

public class TelegramReferralService
{
    private readonly TelegramBotConfig _telegramBotConfig;
    private readonly CommonRewardsConfiguration _commonRewards;
    
    private readonly ITelegramBotClient _botClient;
    private readonly GwardsContext _gwardsContext;

    public TelegramReferralService(
        TelegramBotConfig telegramBotConfig,
        CommonRewardsConfiguration commonRewards,
        ITelegramBotClient botClient,
        GwardsContext gwardsContext
    )
    {
        _telegramBotConfig = telegramBotConfig;
        _commonRewards = commonRewards;
        _botClient = botClient;
        _gwardsContext = gwardsContext;
    }

    public async Task<string> GetReferralLink(UserEntity user)
    {
        if (string.IsNullOrEmpty(user.ReferralCode))
        {
            user.ReferralCode = $"{user.Id}-{Guid.NewGuid():N}";
            await _gwardsContext.SaveChangesAsync();
        }
        
        var queryParams = HttpUtility.ParseQueryString(string.Empty);
        var startAppParams = new
        {
            action = "referral",
            referralCode = user.ReferralCode
        };

        var startAppParamsJson = JsonConvert.SerializeObject(startAppParams);
        var startAppParamsBytes = Encoding.UTF8.GetBytes(startAppParamsJson);
        var startAppParamsB64 = Convert.ToBase64String(startAppParamsBytes);
        
        queryParams.Add("startapp", startAppParamsB64);

        return $"{_telegramBotConfig.TgWebApp}?{queryParams}";
    }

    public async Task PrintReferralMessage(int userId)
    {
        var user = await _gwardsContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            throw new GwardException("User was not found");
        }

        if (user.ChatId == null)
        {
            throw new GwardException("You must have an open chat with the bot to perform this action.");
        }

        var referralLink = await GetReferralLink(user);
        
        var joinButton = new InlineKeyboardButton("Join")
        {
            Url = referralLink,
        };
        
        var controlButtons = new [] { joinButton };
        var controlMarkup = new InlineKeyboardMarkup(controlButtons);
        
        await _botClient.SendTextMessageAsync(
            chatId: user.ChatId,
            text: "Use the message below to invite your friends:"
        );
        await _botClient.SendTextMessageAsync(
            chatId: user.ChatId,
            text: $"Hi, join the Gwards app via join button or using my referral code!\nCode: {user.ReferralCode}",
            replyMarkup: controlMarkup
        );
    }

    public async Task ApplyReferralCode(int userId, string referralCode)
    {
        var user = await _gwardsContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
        {
            throw new GwardException("User was not found");
        }

        if (user.IsReferralCodeApplied)
        {
            throw new GwardException("User cannot use referral code");
        }
        
        var referral = await _gwardsContext.Users.FirstOrDefaultAsync(x => x.ReferralCode == referralCode);
        if (referral == null)
        {
            throw new GwardException("User with this referral code was not found");
        }
        
        if (user.Id == referral.Id)
        {
            throw new GwardException("User cannot use personal referral code");
        }

        referral.GwardPointBalance += _commonRewards.Referral;
        referral.ReferralsInvitedAmount += 1;
        referral.ReferralsPointsRewardAmount += _commonRewards.Referral;
        user.IsReferralCodeApplied = true;

        await _gwardsContext.SaveChangesAsync();
    }
}