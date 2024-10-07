using Gward.Common.Exceptions;
using Gwards.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Gwards.Api.Models.Dto.Responses;

namespace Gwards.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected int ExtractUserId()
    {
        var rawUserIdClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        if (rawUserIdClaim == null)
            throw new GwardException("Auth error");

        if (!int.TryParse(rawUserIdClaim.Value, out int userId))
            throw new GwardException("Auth error");

        return userId;
    }

    protected ApiResponse<T> Ok<T>(T data)
    {
        return new ApiResponse<T>() { Data = data };
    }
}
