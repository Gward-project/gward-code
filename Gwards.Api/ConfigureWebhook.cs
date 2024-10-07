using Gwards.Api.Models;
using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace Gwards.Api;

public class ConfigureWebhook : IHostedService
{
    private readonly ILogger<ConfigureWebhook> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly TelegramBotConfig _telegramBotConfig;

    public ConfigureWebhook(ILogger<ConfigureWebhook> logger, IServiceProvider serviceProvider, TelegramBotConfig telegramBotConfig)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _telegramBotConfig = telegramBotConfig;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();

        var webhookAddress = _telegramBotConfig.WebhookUrl + "/telegram/api";
        await botClient.SetWebhookAsync(
            url: webhookAddress,
            dropPendingUpdates: true,
            cancellationToken: cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var botClient = scope.ServiceProvider.GetRequiredService<ITelegramBotClient>();
        await botClient.DeleteWebhookAsync(cancellationToken: cancellationToken);
    }
}