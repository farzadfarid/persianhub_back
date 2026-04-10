using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Interfaces.Layer3Network;

namespace PersianHub.API.Controllers.Layer3Network;

/// <summary>
/// Manages business reviews submitted by users.
/// Enforces one review per user per business.
/// </summary>
[Route("api/v1/reviews")]
public sealed class ReviewsController(IReviewService reviewService) : ApiControllerBase
{
    /// <summary>Creates a new review for a business.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Create([FromBody] CreateReviewDto request, CancellationToken ct)
    {
        var result = await reviewService.CreateAsync(request, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Returns a review by its internal id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await reviewService.GetByIdAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Returns approved reviews for a specific business (public).</summary>
    [HttpGet("business/{businessId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<ReviewListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByBusiness(int businessId, CancellationToken ct)
    {
        var result = await reviewService.GetByBusinessIdAsync(businessId, ct);
        return MapResult(result);
    }

    /// <summary>Returns all reviews (all statuses) for a business — for the business owner.</summary>
    [HttpGet("business/{businessId:int}/all")]
    [ProducesResponseType(typeof(IReadOnlyList<ReviewListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllByBusiness(int businessId, CancellationToken ct)
    {
        var result = await reviewService.GetAllByBusinessIdAsync(businessId, ct);
        return MapResult(result);
    }

    /// <summary>Approves a review (business owner action).</summary>
    [HttpPatch("{id:int}/approve")]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Approve(int id, CancellationToken ct)
    {
        var result = await reviewService.ApproveAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Rejects a review (business owner action).</summary>
    [HttpPatch("{id:int}/reject")]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Reject(int id, CancellationToken ct)
    {
        var result = await reviewService.RejectAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Deletes a review (business owner action).</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        var result = await reviewService.DeleteAsync(id, ct);
        return result.IsSuccess ? NoContent() : MapResult(result);
    }

    /// <summary>Returns all reviews submitted by a specific user.</summary>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<ReviewListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUser(int userId, CancellationToken ct)
    {
        var result = await reviewService.GetByUserIdAsync(userId, ct);
        return MapResult(result);
    }

    /// <summary>Updates an existing review (rating, title, comment).</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewDto request, CancellationToken ct)
    {
        var result = await reviewService.UpdateAsync(id, request, ct);
        return MapResult(result);
    }
}
