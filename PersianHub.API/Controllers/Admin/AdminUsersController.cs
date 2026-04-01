using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;

namespace PersianHub.API.Controllers.Admin;

/// <summary>
/// Admin management of platform users.
/// </summary>
[Route("api/v1/admin/users")]
[Authorize(Roles = AppRoles.Admin)]
public sealed class AdminUsersController(ApplicationDbContext db) : ApiControllerBase
{
    /// <summary>Returns all users ordered by id descending.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AdminUserListItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var users = await db.AppUsers
            .AsNoTracking()
            .OrderByDescending(u => u.Id)
            .Select(u => new AdminUserListItemDto(
                u.Id, u.Email, u.FirstName, u.LastName, u.Role, u.IsActive, u.CreatedAtUtc))
            .ToListAsync(ct);

        return Ok(users);
    }

    /// <summary>Activates a user account.</summary>
    [HttpPut("{id:int}/activate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Activate(int id, CancellationToken ct)
    {
        var user = await db.AppUsers.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user is null)
            return Problem(detail: $"User with id {id} not found.", title: "Not Found", statusCode: 404);

        user.IsActive = true;
        await db.SaveChangesAsync(ct);

        return NoContent();
    }

    /// <summary>Deactivates a user account.</summary>
    [HttpPut("{id:int}/deactivate")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Deactivate(int id, CancellationToken ct)
    {
        var user = await db.AppUsers.FirstOrDefaultAsync(u => u.Id == id, ct);
        if (user is null)
            return Problem(detail: $"User with id {id} not found.", title: "Not Found", statusCode: 404);

        user.IsActive = false;
        await db.SaveChangesAsync(ct);

        return NoContent();
    }
}
