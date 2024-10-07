using Gwards.Api.Models.Dto.Nft;
using Gwards.Api.Models.Dto.Payments;
using Gwards.Api.Models.Dto.Responses;
using Gwards.Api.Services;
using Gwards.Api.Services.Passport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gwards.Api.Controllers;

[Authorize]
public class PassportController : BaseController
{
    private readonly PassportCoreService _passportCore;
    private readonly PassportExternalsService _passportExternals;
    private readonly PassportPaymentsProcessor _passportPayments;

    public PassportController(
        PassportCoreService passportCore,
        PassportExternalsService passportExternals,
        PassportPaymentsProcessor passportPayments
    )
    {
        _passportCore = passportCore;
        _passportExternals = passportExternals;
        _passportPayments = passportPayments;
    }

    [HttpGet]
    public async Task<ApiResponse<NftPassportInfoDto>> GetUsersPassport()
    {
        var userId = ExtractUserId();
        var passportInfo = await _passportCore.GetInfoByUserId(userId);

        return Ok(passportInfo);
    }
    
    [HttpPost("share")]
    public async Task<IActionResult> ShareNftPassport()
    {
        var userId = ExtractUserId();
        await _passportExternals.PrintShareMessage(userId);

        return Ok();
    }
    
    [HttpGet("validate")]
    public async Task<ApiResponse<bool>> IsUserNftPassportValid()
    {
        var userId = ExtractUserId();
        var isValid = await _passportExternals.IsNftPassportValid(userId);

        return Ok(isValid);
    }

    [HttpPost("mint/start")]
    public async Task<ApiResponse<PaymentInfoDto>> StartPassportMint()
    {
        var userId = ExtractUserId();
        var paymentInfo = await _passportPayments.StartMintPayment(userId);

        return Ok(paymentInfo);
    }
    
    [HttpPost("mint/finalize")]
    public async Task<IActionResult> FinalizePassportMint()
    {
        var userId = ExtractUserId();
        await _passportPayments.FinalizeMintPayment(userId);

        return Ok();
    }
    
    [HttpPut("score-update/start")]
    public async Task<ApiResponse<PaymentInfoDto>> StartPassportScoreUpdate()
    {
        var userId = ExtractUserId();
        var paymentInfo = await _passportPayments.StartMetadataUpdatePayment(userId);

        return Ok(paymentInfo);
    }
    
    [HttpPut("score-update/finalize")]
    public async Task<IActionResult> FinalizePassportScoreUpdate()
    {
        var userId = ExtractUserId();
        await _passportPayments.FinalizeMetadataUpdatePayment(userId);

        return Ok();
    }
}