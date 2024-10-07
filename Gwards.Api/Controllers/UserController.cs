using Gwards.Api.Models.Dto.Responses;
using Gwards.Api.Models.Dto.Users;
using Gwards.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gwards.Api.Controllers;

[Authorize]
public class UserController : BaseController
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ApiResponse<UserInfoDto>> GetMe()
    {
        var userId = ExtractUserId();
        var response = await _userService.GetMe(userId);

        return Ok(response);
    }
    
    [HttpPost("wallet")]
    public async Task<IActionResult> AddUserWalletAddress([FromBody] AddUserWalletDto dto)
    {
        var userId = ExtractUserId();
        await _userService.AddWallet(userId, dto.Address);

        return Ok();
    }
    
    [HttpDelete("wallet")]
    public async Task<IActionResult> RemoveUserWalletAddress()
    {
        var userId = ExtractUserId();
        await _userService.RemoveWallet(userId);

        return Ok();
    }

}