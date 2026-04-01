using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersianHub.API.Auth;
using PersianHub.API.Common;
using PersianHub.API.Data;

namespace PersianHub.API.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize]
public sealed class UsersController(
    ApplicationDbContext db,
    ICurrentUserService currentUser,
    IDateTimeProvider clock) : ControllerBase
{
    /// <summary>Update the authenticated user's profile image URL.</summary>
    [HttpPatch("me/profile-image")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfileImage(
        [FromBody] UpdateProfileImageDto dto,
        CancellationToken ct)
    {
        var userId = currentUser.GetUserId();
        var user = await db.AppUsers.FirstOrDefaultAsync(u => u.Id == userId, ct);
        if (user is null) return NotFound();

        user.ProfileImageUrl = dto.ProfileImageUrl;
        user.UpdatedAtUtc = clock.UtcNow;
        await db.SaveChangesAsync(ct);

        return NoContent();
    }

    /// <summary>Get the authenticated user's profile.</summary>
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMe(CancellationToken ct)
    {
        var userId = currentUser.GetUserId();
        var user = await db.AppUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, ct);

        if (user is null) return NotFound();

        return Ok(new UserProfileDto(
            user.Id, user.Email, user.DisplayName, user.FirstName, user.LastName,
            user.ProfileImageUrl, user.Role));
    }
}

public record UpdateProfileImageDto(string? ProfileImageUrl);

public record UserProfileDto(
    int Id,
    string Email,
    string? DisplayName,
    string? FirstName,
    string? LastName,
    string? ProfileImageUrl,
    string Role);
