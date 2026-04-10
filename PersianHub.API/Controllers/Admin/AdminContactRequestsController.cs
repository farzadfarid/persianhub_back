using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin view of all contact requests across the platform.
/// </summary>
[Route("api/v1/admin/contact-requests")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminContactRequestsController(IContactRequestService contactRequestService) : ApiControllerBase
{
    /// <summary>Returns all contact requests ordered by creation date descending.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ContactRequestListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
        => MapResult(await contactRequestService.GetAllAsync(ct));
}
