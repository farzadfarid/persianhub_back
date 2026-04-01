using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace PersianHub.API.Middlewares;

public sealed class GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            var correlationId = context.Items[CorrelationIdMiddleware.ItemKey] as string ?? "-";
            var userId = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "-";

            logger.LogError(ex,
                "Unhandled exception | Method={Method} Path={Path} UserId={UserId} CorrelationId={CorrelationId}",
                context.Request.Method, context.Request.Path, userId, correlationId);

            await WriteErrorResponseAsync(context, correlationId);
        }
    }

    private static async Task WriteErrorResponseAsync(HttpContext context, string correlationId)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var problem = new
        {
            type = "https://tools.ietf.org/html/rfc7807",
            title = "Internal Server Error",
            status = 500,
            detail = "An unexpected error occurred. Please try again later.",
            correlationId
        };

        var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        await context.Response.WriteAsync(json);
    }
}
