using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Interfaces.Layer3Network;

namespace PersianHub.API.Controllers.Layer3Network;

/// <summary>
/// User reactions on reviews (Like / Helpful / Love).
/// One reaction per type per user per review — idempotent add.
/// </summary>
[Route("api/v1/review-reactions")]
public sealed class ReviewReactionsController(IReviewReactionService reactionService) : ApiControllerBase
{
    /// <summary>Adds a reaction to a review. Idempotent — returns existing if already reacted with same type.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReviewReactionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Add([FromBody] CreateReviewReactionDto request, CancellationToken ct)
    {
        var result = await reactionService.AddAsync(request, ct);
        return MapCreated(result, null, null);
    }

    /// <summary>Removes a reaction by id.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remove(int id, CancellationToken ct)
        => MapResult(await reactionService.RemoveAsync(id, ct));

    /// <summary>Returns all reactions by a user (used to determine current user's reaction state).</summary>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<ReviewReactionDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUser(int userId, CancellationToken ct)
        => MapResult(await reactionService.GetByUserAsync(userId, ct));
}
