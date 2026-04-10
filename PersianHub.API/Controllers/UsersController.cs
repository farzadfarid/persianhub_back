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
            user.Id, user.Email,
            user.DisplayName, user.DisplayNameFa,
            user.FirstName, user.FirstNameFa,
            user.LastName, user.LastNameFa,
            user.Bio, user.BioFa,
            user.ProfileImageUrl, user.Role));
    }

    /// <summary>Update the authenticated user's profile.</summary>
    [HttpPatch("me/profile")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateUserProfileDto dto,
        CancellationToken ct)
    {
        var userId = currentUser.GetUserId();
        var user = await db.AppUsers.FirstOrDefaultAsync(u => u.Id == userId, ct);
        if (user is null) return NotFound();

        user.FirstName = dto.FirstName ?? user.FirstName;
        user.FirstNameFa = dto.FirstNameFa;
        user.LastName = dto.LastName ?? user.LastName;
        user.LastNameFa = dto.LastNameFa;
        user.DisplayName = dto.DisplayName;
        user.DisplayNameFa = dto.DisplayNameFa;
        user.Bio = dto.Bio;
        user.BioFa = dto.BioFa;
        user.UpdatedAtUtc = clock.UtcNow;
        await db.SaveChangesAsync(ct);

        return NoContent();
    }
}

public record UpdateProfileImageDto(string? ProfileImageUrl);

public record UpdateUserProfileDto(
    string? FirstName,
    string? FirstNameFa,
    string? LastName,
    string? LastNameFa,
    string? DisplayName,
    string? DisplayNameFa,
    string? Bio,
    string? BioFa
);

public record UserProfileDto(
    int Id,
    string Email,
    string? DisplayName,
    string? DisplayNameFa,
    string? FirstName,
    string? FirstNameFa,
    string? LastName,
    string? LastNameFa,
    string? Bio,
    string? BioFa,
    string? ProfileImageUrl,
    string Role);
