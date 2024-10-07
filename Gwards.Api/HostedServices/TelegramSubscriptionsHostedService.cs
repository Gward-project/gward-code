using Gward.Common.Constants;
using Gwards.DAL;
using Gwards.DAL.Enums;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Gwards.Api.HostedServices;

public class TelegramSubscriptionsHostedService : BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly ITelegramBotClient _botClient;
    private readonly GwardsContext _dbContext;
    private readonly IServiceScope _scope;
    
    private const int SubscriptionCheckDelayInSeconds = 2 * 1000;
    private const int GlobalSubscriptionCheckDelayInMs = 5 * 60 * 1000;

    public TelegramSubscriptionsHostedService(IServiceProvider serviceProvider)
    {
        _scope = serviceProvider.CreateScope();
        _configuration = _scope.ServiceProvider.GetRequiredService<IConfiguration>();
        _botClient = _scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        _dbContext = _scope.ServiceProvider.GetRequiredService<GwardsContext>();
    }
        
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var gwardPublicChannelId = _configuration["GwardPublicChannelId"];
        if (gwardPublicChannelId == null)
        {
            throw new ApplicationException("Gward public channel ID was not provided");
        }

        var isBotInChat = await IsBotInChat(gwardPublicChannelId, cancellationToken);
        if (!isBotInChat)
        {
            _scope.Dispose();
            return;
        }

        while (!cancellationToken.IsCancellationRequested)
        {
            var usersSubscribeQuests = await _dbContext
                .UserQuests
                .Include(x => x.User)
                .Include(x => x.Quest)
                .Where(x => x.Status == QuestStatus.New && x.Quest.Id == CommonQuestsIds.SubscribeOnTelegram)
                .ToListAsync(cancellationToken);

            foreach (var subscriptionQuest in usersSubscribeQuests)
            {
                var isMember = await IsUserMemberOfChat(gwardPublicChannelId, subscriptionQuest.User.TelegramId, cancellationToken);
                if (isMember)
                {
                    subscriptionQuest.Status = QuestStatus.ReadyForClaim;
                }
                
                await Task.Delay(SubscriptionCheckDelayInSeconds, cancellationToken);
            }
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            await Task.Delay(GlobalSubscriptionCheckDelayInMs, cancellationToken);
        }

        _scope.Dispose();
    }

    private async Task<bool> IsBotInChat(string chatId, CancellationToken cancellationToken)
    {
        try
        {
            var bot = await _botClient.GetMeAsync(cancellationToken);
            var member = await _botClient.GetChatMemberAsync(chatId, bot.Id, cancellationToken);

            return member.Status == ChatMemberStatus.Administrator;
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> IsUserMemberOfChat(string chatId, long userId, CancellationToken cancellationToken)
    {
        try
        {
            var member = await _botClient.GetChatMemberAsync(chatId, userId, cancellationToken);
            return member.Status == ChatMemberStatus.Creator ||
                   member.Status == ChatMemberStatus.Administrator ||
                   member.Status == ChatMemberStatus.Member;
        }
        catch
        {
            return false;
        }
    }
}