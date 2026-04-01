using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Interfaces.Layer1Hook;

namespace PersianHub.API.Controllers.Layer1Hook;

/// <summary>
/// Records and retrieves user activity events for behavioral analytics and personalization.
/// Activities use a polymorphic ReferenceType + ReferenceId pattern.
/// </summary>
[Route("api/v1/user-activities")]
public sealed class UserActivitiesController(IUserActivityService userActivityService) : ApiControllerBase
{
    /// <summary>Records a new user activity event.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(UserActivityDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateUserActivityDto request, CancellationToken ct)
    {
        var result = await userActivityService.CreateAsync(request, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Returns a user activity by its internal id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(UserActivityDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await userActivityService.GetByIdAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Returns all activity events recorded for a specific user.</summary>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<UserActivityListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUser(int userId, CancellationToken ct)
    {
        var result = await userActivityService.GetByUserIdAsync(userId, ct);
        return MapResult(result);
    }

    /// <summary>Returns the N most recent activities for a user. Defaults to 20 if count is not specified.</summary>
    [HttpGet("user/{userId:int}/recent")]
    [ProducesResponseType(typeof(IReadOnlyList<UserActivityListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecentByUser(int userId, [FromQuery] int count = 20, CancellationToken ct = default)
    {
        var result = await userActivityService.GetRecentByUserIdAsync(userId, count, ct);
        return MapResult(result);
    }
}
