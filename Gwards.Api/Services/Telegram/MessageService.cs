using Gwards.Api.Models;
using Gwards.Api.Services.Common;
using Gwards.DAL;
using Gwards.DAL.Entities;
using Gwards.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Gwards.Api.Services.Telegram;

public class MessageService
{
    private readonly ITelegramBotClient _botClient;
    private readonly TelegramBotConfig _telegramBotConfig;

    private readonly IServiceProvider _serviceProvider; 

    public MessageService(
        ITelegramBotClient botClient,
        TelegramBotConfig telegramBotConfig,
        IServiceProvider serviceProvider
    )
    {
        _botClient = botClient;
        _telegramBotConfig = telegramBotConfig;
        _serviceProvider = serviceProvider;
    }

    public async Task HandleMessage(Update update)
    {
        if (update.Type == UpdateType.CallbackQuery)
        {
            await HandleCallbackQuery(update);
        }
        else
        {
            await HandleTextMessage(update);
        }
    }

    private Task HandleCallbackQuery(Update update)
    {
        return Task.CompletedTask;
    }

    private async Task HandleTextMessage(Update update)
    {
        if (update.Message == null || update.Message.From == null)
        {
            return;
        }

        await HandleAnyMessage(update.Message);

        if (update.Message.Text == "/start")
        {
            await HandleStartCommand(update.Message);
        }
    }

    private async Task HandleAnyMessage(Message message)
    {
        if (message.From == null)
        {
            return;
        }
        
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GwardsContext>();
        
        var chatId = message.Chat.Id;
        var telegramUserId = message.From.Id;
        var user = await dbContext.Users.FirstOrDefaultAsync(x => x.TelegramId == telegramUserId);

        if (user == null)
        {
            var defaultQuests = await dbContext
                .Quests
                .Where(x => x.IsDefault)
                .ToListAsync();
            
            user = new UserEntity
            {
                TelegramId = telegramUserId,
                ChatId = chatId,
                Nickname = message.From.Username,
                CreatedAt = DateTime.UtcNow,
            };
            
            user.UserQuests = defaultQuests.Select(x => new UserQuestEntity
            {
                User = user,
                Quest = x,
                CreatedAt = DateTime.UtcNow,
                Status = QuestStatus.New,
            }).ToList();

            dbContext.Users.Add(user);
            
        }

        user.ChatId ??= chatId;

        if (dbContext.ChangeTracker.HasChanges())
        {
            await dbContext.SaveChangesAsync();   
        }
        
        var menuButton = new MenuButtonWebApp
        {
            Text = "Gward",
            WebApp = new WebAppInfo { Url = _telegramBotConfig.WebApp }
        };

        await _botClient.SetChatMenuButtonAsync(chatId, menuButton);
    }

    private async Task HandleStartCommand(Message message)
    {
        using var scope = _serviceProvider.CreateScope();
        var fileStorage = scope.ServiceProvider.GetRequiredService<FileStorageService>();

        await using var image = fileStorage.Open("Images/welcome.jpg");
        var caption = """
                      *Welcome to Gward! 🐍*
                      
                      Get ready for a journey with Gward Bot, where your love for games turns into awesome rewards 💰
                      
                      *Here’s what you can do:*
                      
                      💲 Farm points every day by checking-in every day! 
                      👍 Complete quests in Steam games and mint NFT rewards!
                      🪪 Check your Web2 Score and mint Gward Passport!
                      🎮 Invite your friends - earn points for every new referral!
                      
                       
                      Wondering what you can do with your points? 
                      Subscribe to our telegram channel and stay tuned to find out! 🐘
                      """;
        
        var gwardButton = new InlineKeyboardButton("Gward")
        {
            Url = _telegramBotConfig.TgWebApp,
        };

        var gwardNewsButton = new InlineKeyboardButton("Gward news")
        {
            Url = "https://t.me/gwardxyz"
        };
        
        var controlButtons = new [] { gwardButton, gwardNewsButton };
        var controlMarkup = new InlineKeyboardMarkup(controlButtons);
        
        await _botClient.SendPhotoAsync(
            message.Chat.Id,
            photo: new InputFileStream(image),
            caption: caption,
            parseMode: ParseMode.Markdown,
            replyMarkup: controlMarkup
        );
    }
}
