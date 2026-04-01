namespace PersianHub.API.Middlewares;

/// <summary>
/// Assigns a correlation ID to every request.
///
/// Rules:
/// - If the client sends X-Correlation-Id and it is a valid GUID, reuse it.
/// - Otherwise generate a new one.
/// - Stored in HttpContext.Items["CorrelationId"] for downstream access.
/// - Returned as X-Correlation-Id response header so clients can include it in support requests.
/// </summary>
public sealed class CorrelationIdMiddleware(RequestDelegate next)
{
    public const string HeaderName = "X-Correlation-Id";
    public const string ItemKey = "CorrelationId";

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = ResolveCorrelationId(context);
        context.Items[ItemKey] = correlationId;
        context.Response.OnStarting(() =>
        {
            context.Response.Headers[HeaderName] = correlationId;
            return Task.CompletedTask;
        });

        await next(context);
    }

    private static string ResolveCorrelationId(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue(HeaderName, out var incoming))
        {
            var value = incoming.ToString();
            // Accept only valid GUIDs to prevent header injection.
            if (Guid.TryParse(value, out var parsed))
                return parsed.ToString("D");
        }

        return Guid.NewGuid().ToString("D");
    }
}
