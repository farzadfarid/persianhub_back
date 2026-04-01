using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin moderation of user-submitted business suggestions.
/// Approving a suggestion creates a Business record (no owner assigned — use claim flow to assign ownership).
/// </summary>
[Route("api/v1/admin/business-suggestions")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminBusinessSuggestionsController(IAdminBusinessSuggestionService suggestionService) : ApiControllerBase
{
    /// <summary>Returns all business suggestions ordered by submission date.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<BusinessSuggestionListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => MapResult(await suggestionService.GetAllAsync(ct));

    /// <summary>Returns full details for a business suggestion.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BusinessSuggestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await suggestionService.GetByIdAsync(id, ct));

    /// <summary>
    /// Approves a suggestion and creates a Business record from it.
    /// The business has no owner (IsClaimed = false) — ownership must be assigned separately via a claim request.
    /// </summary>
    [HttpPatch("{id:int}/approve")]
    [ProducesResponseType(typeof(BusinessSuggestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Approve(int id, CancellationToken ct)
        => MapResult(await suggestionService.ApproveAsync(id, ct));

    /// <summary>Rejects a suggestion. Status updated; no business record created.</summary>
    [HttpPatch("{id:int}/reject")]
    [ProducesResponseType(typeof(BusinessSuggestionDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Reject(int id, CancellationToken ct)
        => MapResult(await suggestionService.RejectAsync(id, ct));
}
