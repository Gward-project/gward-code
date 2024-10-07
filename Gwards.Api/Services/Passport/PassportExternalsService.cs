using Gward.Common.Exceptions;
using Gwards.Api.Models;
using Gwards.Api.Services.Common;
using Gwards.Api.Services.Ton;
using Gwards.DAL;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TonSdk.Core;

namespace Gwards.Api.Services.Passport;

public class PassportExternalsService
{
    private readonly TelegramBotConfig _botConfig;
    private readonly ITelegramBotClient _botClient;

    private readonly PassportCoreService _passportCoreService;
    private readonly FileStorageService _fileStorageService;
    private readonly TonService _tonService;
    
    private readonly GwardsContext _dbContext;

    public PassportExternalsService(
        TelegramBotConfig botConfig,
        ITelegramBotClient botClient,
        PassportCoreService passportCoreService,
        FileStorageService fileStorageService,
        TonService tonService,
        GwardsContext dbContext
    )
    {
        _botConfig = botConfig;
        _botClient = botClient;
        _passportCoreService = passportCoreService;
        _fileStorageService = fileStorageService;
        _tonService = tonService;
        _dbContext = dbContext;
    }

    public async Task PrintShareMessage(int userId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            throw new GwardException("User was not found");
        }

        if (user.ChatId == null)
        {
            throw new GwardException("User must have telegram chat with bot");
        }

        var passportInfo = await _passportCoreService.GetInfoByMinterAddress(user.TonAddress);
        if (passportInfo.Address == null)
        {
            throw new GwardException("User doesn't own NFT passport");
        }
        
        var joinButton = new InlineKeyboardButton("Gward")
        {
            Url = $"{_botConfig.TgWebApp}"
        };

        var fileStream = _fileStorageService.Open(passportInfo.ImagePath);
        var controlButtons = new [] { joinButton };
        var controlMarkup = new InlineKeyboardMarkup(controlButtons);
        
        await _botClient.SendPhotoAsync(
            chatId: user.ChatId,
            photo: InputFile.FromStream(fileStream), 
            caption: "My Gward's NFT passport!",
            replyMarkup: controlMarkup
        );
    }
    
    public async Task<bool> IsNftPassportValid(int userId)
    {
        var user = await _dbContext
            .Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            throw new GwardException("User was not found");
        }

        var passport = await _dbContext.NftPassports.FirstOrDefaultAsync(x => x.MinterAddress == user.TonAddress);
        if (passport == null)
        {
            throw new GwardException("User doesn't own NFT passport");
        }

        var isPassportValid = true;
        var userAddress = new Address(user.TonAddress);
        var passportAddress = new Address(passport.Address);
        var passportOnChainData = await _tonService.TonClient.Nft.GetNftItemData(passportAddress);
        
        // NFT was burned
        if (passportOnChainData.OwnerAddress != userAddress)
        {
            isPassportValid = false;
            _dbContext.Remove(passport);
        }

        // NFT was offloaded from the blockchain
        if (!passportOnChainData.Init)
        {
            isPassportValid = false;
            _dbContext.Remove(passport);
        }

        if (_dbContext.ChangeTracker.HasChanges())
        {
            await _dbContext.SaveChangesAsync();
        }

        return isPassportValid;
    }
}