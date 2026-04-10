using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

/// <summary>
/// Admin visibility into the invite flow. Read-only — invites are user-initiated.
/// </summary>
public sealed class AdminInviteService(ApplicationDbContext db) : IAdminInviteService
{
    public async Task<PagedResult<AdminInviteListItemDto>> GetAllAsync(
        int? inviterUserId, InviteStatus? status, InviteChannel? channel,
        int page, int pageSize, CancellationToken ct)
    {
        var query = db.Invites
            .AsNoTracking()
            .Include(i => i.InviterUser)
            .AsQueryable();

        if (inviterUserId.HasValue)
            query = query.Where(i => i.InviterUserId == inviterUserId.Value);

        if (status.HasValue)
            query = query.Where(i => i.Status == status.Value);

        if (channel.HasValue)
            query = query.Where(i => i.Channel == channel.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(i => i.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(i => new AdminInviteListItemDto(
                i.Id,
                i.InviterUserId, i.InviterUser.Email,
                i.InviteeEmail, i.InviteePhoneNumber,
                i.Channel, i.Status,
                i.SentAtUtc, i.AcceptedAtUtc))
            .ToListAsync(ct);

        return new PagedResult<AdminInviteListItemDto>(items, total, page, pageSize);
    }

    public async Task<Result<AdminInviteDetailDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var invite = await db.Invites
            .AsNoTracking()
            .Include(i => i.InviterUser)
            .FirstOrDefaultAsync(i => i.Id == id, ct);

        if (invite is null)
            return Result<AdminInviteDetailDto>.Failure("Invite not found.", ErrorCodes.NotFound);

        return Result<AdminInviteDetailDto>.Success(new AdminInviteDetailDto(
            invite.Id,
            invite.InviterUserId, invite.InviterUser.Email,
            invite.InviteeEmail, invite.InviteePhoneNumber,
            invite.Channel, invite.Status,
            invite.SentAtUtc, invite.AcceptedAtUtc,
            invite.CreatedAtUtc, invite.UpdatedAtUtc));
    }
}
