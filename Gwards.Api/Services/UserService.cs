using Gward.Common.Constants;
using Gward.Common.Exceptions;
using Gwards.Api.Constants;
using Gwards.Api.Models.Configurations;
using Gwards.Api.Models.Dto.Users;
using Gwards.Api.Services.Common;
using Gwards.Api.Services.Telegram;
using Gwards.Api.Utils;
using Gwards.DAL;
using Gwards.DAL.Entities;
using Gwards.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using TonSdk.Core;

namespace Gwards.Api.Services;

public class UserService
{
    private readonly ITelegramBotClient _botClient;
    private readonly TelegramReferralService _referralService;
    private readonly FileStorageService _fileStorage;
    
    private readonly GwardsContext _gwardsContext;

    public UserService(
        ITelegramBotClient botClient,
        TelegramReferralService referralService,
        FileStorageService fileStorage,
        GwardsContext gwardsContext
    )
    {
        _botClient = botClient;
        _referralService = referralService;
        _fileStorage = fileStorage;
        _gwardsContext = gwardsContext;
    }

    public async Task<UserInfoDto> GetMe(int userId)
    {
        var user = await _gwardsContext
            .Users
            .Include(x => x.ExternalAccounts)
            .Include(x => x.DailyReward)
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            throw new GwardException("User not found");
        }

        var referralLink = await _referralService.GetReferralLink(user);
        await FetchUserAvatar(user);
            
        return new UserInfoDto
        {
            Id = user.Id,
            TelegramId = user.TelegramId,
            Nickname = user.Nickname,
            AvatarPath = user.AvatarPath,
            TonAddress = user.TonAddress,
            GwardPointBalance = user.GwardPointBalance,
            Score = user.Score,
            ReferralsInvitedAmount = user.ReferralsInvitedAmount,
            ReferralsPointsRewardAmount = user.ReferralsPointsRewardAmount,
            ReferralLink = referralLink,
            DailyRewardAmount = DailyRewardsUtil.GetReward(user.DailyReward.Day),
            DailyRewardDaysClaimed = user.DailyReward.Day,
            DailyRewardAvailableAt = user.DailyReward.LastRewardClaimedAt + TimeSpan.FromDays(1),
            HasLinkedSteamAccount = user.ExternalAccounts.Any(x => x.Type == "steam")
        };
    }

    public async Task AddWallet(int userId, string walletAddress)
    {
        var user = await _gwardsContext
            .Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            throw new GwardException("User not found");
        }

        if (user.TonAddress != null)
        {
            throw new GwardException("User already has wallet address");
        }
        
        var userWithExistingAddress = await _gwardsContext
            .Users
            .FirstOrDefaultAsync(x => x.TonAddress == walletAddress);

        if (userWithExistingAddress != null)
        {
            throw new GwardException("User with provided ton address already exists in the system");
        }

        if (!TonUtil.IsValidTonAddress(walletAddress))
        {
            throw new GwardException("Invalid ton address was provided");
        }

        var connectTonWalletQuest = await _gwardsContext
            .UserQuests
            .FirstOrDefaultAsync(x =>
                x.Quest.Id == CommonQuestsIds.ConnectTelegramWallet &&
                x.Status == QuestStatus.New &&
                x.UserId == userId
            );

        if (connectTonWalletQuest != null)
        {
            connectTonWalletQuest.Status = QuestStatus.ReadyForClaim;
        }

        user.TonAddress = new Address(walletAddress).ToString();
        await _gwardsContext.SaveChangesAsync();
    }

    public async Task RemoveWallet(int userId)
    {
        var user = await _gwardsContext
            .Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            throw new GwardException("User not found");
        }

        if (user.TonAddress == null)
        {
            throw new GwardException("User doesn't have wallet address");
        }

        user.TonAddress = null;
        await _gwardsContext.SaveChangesAsync();
    }
        
    private async Task FetchUserAvatar(UserEntity user)
    {
        var refetchAvailableAt = user.AvatarFetchedAt + TimeSpan.FromDays(UserConstants.AvatarPersistingDays);
        if (refetchAvailableAt > DateTime.UtcNow)
        {
            return;
        }
        
        var userProfilePhotos = await _botClient.GetUserProfilePhotosAsync(user.TelegramId);
        if (userProfilePhotos.Photos.Length == 0)
        {
            return;
        }

        var latestPhotoId = userProfilePhotos.Photos[0][1].FileId;
        using var memoryStream = new MemoryStream();
        await _botClient.GetInfoAndDownloadFileAsync(latestPhotoId, memoryStream);
        
        if (!string.IsNullOrEmpty(user.AvatarPath) && _fileStorage.IsExist(user.AvatarPath))
        {
            _fileStorage.Delete(user.AvatarPath);
        }
        
        var avatarFilePath = $"/static/Avatars/{user.Id}.jpeg";
        memoryStream.Position = 0;
        _fileStorage.Save(memoryStream, avatarFilePath);
        
        user.AvatarPath = avatarFilePath;
        user.AvatarFetchedAt = DateTime.UtcNow;
        await _gwardsContext.SaveChangesAsync();
    }
}