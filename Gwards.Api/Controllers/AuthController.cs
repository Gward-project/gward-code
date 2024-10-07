using Gwards.Api.Models;
using Gwards.Api.Services.Telegram;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gwards.Api.Controllers;

[Authorize]
public class AuthController : BaseController
{
    private readonly TelegramAuthService _telegramAuthService;
    private readonly TelegramReferralService _telegramReferralService;

    public AuthController(
        TelegramAuthService telegramAuthService,
        TelegramReferralService telegramReferralService
    )
    {
        _telegramAuthService = telegramAuthService;
        _telegramReferralService = telegramReferralService;
    }

    [HttpGet("signin")]
    [AllowAnonymous]
    public async Task<SignInResponse> SignIn(string initData)
    {
        return await _telegramAuthService.SignIn(initData);
    }
    
    [HttpGet("referral")]
    public async Task PrintReferralMessage()
    {
        var userId = ExtractUserId();
        await _telegramReferralService.PrintReferralMessage(userId);
    }
    
    [HttpPut("referral")]
    public async Task ApplyReferralCode(string referralCode)
    {
        var userId = ExtractUserId();
        await _telegramReferralService.ApplyReferralCode(userId, referralCode);
    }
}
