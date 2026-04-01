using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Interfaces.Layer1Hook;

namespace PersianHub.API.Controllers.Layer1Hook;

/// <summary>
/// Manages user notifications. Supports read tracking and bulk mark-all-read.
/// </summary>
[Route("api/v1/notifications")]
public sealed class NotificationsController(INotificationService notificationService) : ApiControllerBase
{
    /// <summary>Creates a new notification for a user.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateNotificationDto request, CancellationToken ct)
    {
        var result = await notificationService.CreateAsync(request, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Returns a notification by its internal id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(NotificationDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await notificationService.GetByIdAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Returns all notifications for a specific user.</summary>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<NotificationListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUser(int userId, CancellationToken ct)
    {
        var result = await notificationService.GetByUserIdAsync(userId, ct);
        return MapResult(result);
    }

    /// <summary>Returns only unread notifications for a specific user.</summary>
    [HttpGet("user/{userId:int}/unread")]
    [ProducesResponseType(typeof(IReadOnlyList<NotificationListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUnreadByUser(int userId, CancellationToken ct)
    {
        var result = await notificationService.GetUnreadByUserIdAsync(userId, ct);
        return MapResult(result);
    }

    /// <summary>Marks a single notification as read.</summary>
    [HttpPatch("{id:int}/read")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsRead(int id, CancellationToken ct)
    {
        var result = await notificationService.MarkAsReadAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Marks all notifications for a user as read in one operation.</summary>
    [HttpPatch("user/{userId:int}/read-all")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAllAsRead(int userId, CancellationToken ct)
    {
        var result = await notificationService.MarkAllAsReadAsync(userId, ct);
        return MapResult(result);
    }
}
