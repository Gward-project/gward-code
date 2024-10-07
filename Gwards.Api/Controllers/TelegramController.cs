using Gwards.Api.Services;
using Gwards.Api.Services.Telegram;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Gwards.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TelegramController : BaseController
{
    private readonly MessageService _messageService;

    public TelegramController(ITelegramBotClient botClient, MessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost("api")]
    public async Task<IActionResult> Post(Update update)
    {
        if (update.Type != UpdateType.CallbackQuery && update.Type != UpdateType.Message)
            return Ok();

        await _messageService.HandleMessage(update);

        return Ok();
    }
}
