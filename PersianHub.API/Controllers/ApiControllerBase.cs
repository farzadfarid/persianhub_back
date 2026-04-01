using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;

namespace PersianHub.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>Maps a void Result to an HTTP response (204 on success).</summary>
    protected IActionResult MapResult(Result result)
    {
        if (result.IsSuccess) return NoContent();
        return MapError(result.Error!, result.ErrorCode);
    }

    /// <summary>Maps a Result&lt;T&gt; to 200 OK on success.</summary>
    protected IActionResult MapResult<T>(Result<T> result)
    {
        if (result.IsSuccess) return Ok(result.Value);
        return MapError(result.Error!, result.ErrorCode);
    }

    /// <summary>Maps a Result&lt;T&gt; to 201 Created on success.</summary>
    protected IActionResult MapCreated<T>(Result<T> result, string? actionName, object? routeValues)
    {
        if (result.IsSuccess) return CreatedAtAction(actionName, routeValues, result.Value);
        return MapError(result.Error!, result.ErrorCode);
    }

    private IActionResult MapError(string detail, string? errorCode) => errorCode switch
    {
        ErrorCodes.NotFound => Problem(detail: detail, title: "Not Found", statusCode: 404),
        ErrorCodes.AlreadyExists => Problem(detail: detail, title: "Conflict", statusCode: 409),
        ErrorCodes.Conflict => Problem(detail: detail, title: "Conflict", statusCode: 409),
        ErrorCodes.ValidationFailed => Problem(detail: detail, title: "Bad Request", statusCode: 400),
        ErrorCodes.Forbidden => Problem(detail: detail, title: "Forbidden", statusCode: 403),
        _ => Problem(detail: "An unexpected error occurred.", title: "Internal Server Error", statusCode: 500)
    };
}
