using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Interfaces.Layer1Hook;

namespace PersianHub.API.Controllers.Layer1Hook;

/// <summary>
/// Manages user favorites (saved businesses, events, offers, listings).
/// Uses a polymorphic ReferenceType + ReferenceId pattern.
/// </summary>
[Route("api/v1/favorites")]
public sealed class FavoritesController(IFavoriteService favoriteService) : ApiControllerBase
{
    /// <summary>Adds an item to a user's favorites.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(FavoriteDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Add([FromBody] AddFavoriteDto request, CancellationToken ct)
    {
        var result = await favoriteService.AddAsync(request, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Returns a favorite by its internal id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FavoriteDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await favoriteService.GetByIdAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Removes a favorite by its internal id.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(int id, CancellationToken ct)
    {
        var result = await favoriteService.RemoveAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Returns all favorites for a specific user.</summary>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<FavoriteListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUser(int userId, CancellationToken ct)
    {
        var result = await favoriteService.GetByUserIdAsync(userId, ct);
        return MapResult(result);
    }
}
