using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin read-only view of the platform audit trail.
/// Audit logs are immutable — no create, update, or delete endpoints.
/// </summary>
[Route("api/v1/admin/audit-logs")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminAuditLogsController(IAdminAuditLogService auditLogService) : ApiControllerBase
{
    /// <summary>Paginated list of audit log entries with optional filters.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? performedByUserId,
        [FromQuery] string? action,
        [FromQuery] string? entityType,
        [FromQuery] string? entityId,
        [FromQuery] string? correlationId,
        [FromQuery] DateTime? fromUtc,
        [FromQuery] DateTime? toUtc,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await auditLogService.GetAllAsync(
            performedByUserId, action, entityType, entityId,
            correlationId, fromUtc, toUtc, page, pageSize, ct);
        return Ok(result);
    }

    /// <summary>Returns full details for a single audit log entry by id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AdminAuditLogDetailDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
        => MapResult(await auditLogService.GetByIdAsync(id, ct));
}
