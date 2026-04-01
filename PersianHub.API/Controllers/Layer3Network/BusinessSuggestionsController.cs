using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Interfaces.Layer3Network;

namespace PersianHub.API.Controllers.Layer3Network;

/// <summary>
/// Manages community-submitted business suggestions.
/// Supports an admin review workflow: Pending → Approved / Rejected.
/// </summary>
[Route("api/v1/business-suggestions")]
public sealed class BusinessSuggestionsController(IBusinessSuggestionService businessSuggestionService) : ApiControllerBase
{
    /// <summary>Submits a new business suggestion.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(BusinessSuggestionDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateBusinessSuggestionDto request, CancellationToken ct)
    {
        var result = await businessSuggestionService.CreateAsync(request, ct);
        return MapCreated(result, nameof(GetById), new { id = result.Value?.Id });
    }

    /// <summary>Returns a business suggestion by its internal id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BusinessSuggestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var result = await businessSuggestionService.GetByIdAsync(id, ct);
        return MapResult(result);
    }

    /// <summary>Returns all business suggestions submitted by a specific user.</summary>
    [HttpGet("user/{userId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<BusinessSuggestionListItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByUser(int userId, CancellationToken ct)
    {
        var result = await businessSuggestionService.GetByUserIdAsync(userId, ct);
        return MapResult(result);
    }

    /// <summary>Returns all business suggestions (admin use).</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<BusinessSuggestionListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await businessSuggestionService.GetAllAsync(ct);
        return MapResult(result);
    }

    /// <summary>Updates the review status of a business suggestion (admin action).</summary>
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateBusinessSuggestionStatusDto request, CancellationToken ct)
    {
        var result = await businessSuggestionService.UpdateStatusAsync(id, request, ct);
        return MapResult(result);
    }
}
