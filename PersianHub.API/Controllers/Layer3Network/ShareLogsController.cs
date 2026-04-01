using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Interfaces.Layer3Network;

namespace PersianHub.API.Controllers.Layer3Network;

/// <summary>
/// Records and retrieves share events for the growth engine.
/// Tracks when users share businesses, events, deals, or listings via various channels.
/// </summary>
[Route("api/v1/share-logs")]
public sealed class ShareLogsController(IShareLogService shareLogService) : ApiControllerBase
{
    /// <summary>Records a new share event.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ShareLogDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateShareLogDto request, CancellationToken ct)
    {
        var result = await shareLogService.CreateAsync(request, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Returns a share log entry by its internal id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ShareLogDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await shareLogService.GetByIdAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Returns all share logs recorded for a specific user.</summary>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<ShareLogListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUser(int userId, CancellationToken ct)
    {
        var result = await shareLogService.GetByUserIdAsync(userId, ct);
        return MapResult(result);
    }
}
