using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin view of all interactions across the platform.
/// </summary>
[Route("api/v1/admin/interactions")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminInteractionsController(IInteractionService interactionService) : ApiControllerBase
{
    /// <summary>Returns all interactions ordered by creation date descending.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<InteractionListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => MapResult(await interactionService.GetAllAsync(ct));
}
