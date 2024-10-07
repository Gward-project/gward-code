using Gward.Common.Exceptions;
using Gwards.Api.Models;
using System.Net;
using Gwards.Api.Models.Dto.Responses;

namespace Gwards.Api.MIddlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            if (e is GwardException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogError(e, "InternalError:");
            }

            var response = new ApiResponse<object> { Error = e.Message };
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}