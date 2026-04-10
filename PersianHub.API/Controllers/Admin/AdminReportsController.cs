using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin moderation of Reports.
/// ReportStatus lifecycle: Pending → Reviewed (mark-reviewed) → ActionTaken (resolve) | Dismissed (dismiss).
/// Reports are the main moderation intake channel — admin does not create reports.
/// </summary>
[Route("api/v1/admin/reports")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminReportsController(IAdminReportService reportService) : ApiControllerBase
{
    /// <summary>Paginated list of all reports with optional filters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] ReportStatus? status,
        [FromQuery] string? referenceType,
        [FromQuery] int? referenceId,
        [FromQuery] int? userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await reportService.GetAllAsync(status, referenceType, referenceId, userId, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>Returns full details for a report by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AdminReportDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await reportService.GetByIdAsync(id, ct));

    /// <summary>Marks a report as reviewed — sets status to Reviewed. Only valid from Pending.</summary>
    [HttpPatch("{id:int}/mark-reviewed")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkReviewed(int id, CancellationToken ct)
        => MapResult(await reportService.MarkReviewedAsync(id, ct));

    /// <summary>Resolves a report — sets status to ActionTaken.</summary>
    [HttpPatch("{id:int}/resolve")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Resolve(int id, CancellationToken ct)
        => MapResult(await reportService.ResolveAsync(id, ct));

    /// <summary>Dismisses a report — sets status to Dismissed.</summary>
    [HttpPatch("{id:int}/dismiss")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Dismiss(int id, CancellationToken ct)
        => MapResult(await reportService.DismissAsync(id, ct));
}
