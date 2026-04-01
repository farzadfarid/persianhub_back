using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PersianHub.API.Middlewares;

/// <summary>
/// Logs each request with method, path, status code, duration, user id, and correlation id.
///
/// Privacy rules applied:
/// - Request bodies are NOT logged.
/// - Authorization headers are NOT logged.
/// - Only the user id (numeric claim) is included — no email, no role, no token value.
/// </summary>
public sealed class RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        var correlationId = context.Items[CorrelationIdMiddleware.ItemKey] as string ?? "-";

        try
        {
            await next(context);
        }
        finally
        {
            sw.Stop();
            var userId = context.User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? "-";
            logger.LogInformation(
                "HTTP {Method} {Path} responded {StatusCode} in {ElapsedMs}ms | user={UserId} corr={CorrelationId}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                sw.ElapsedMilliseconds,
                userId,
                correlationId);
        }
    }
}
