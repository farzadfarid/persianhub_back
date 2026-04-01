using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Entities.Layer3Network;
using PersianHub.API.Enums.Layer3Network;
using PersianHub.API.Interfaces.Layer3Network;

namespace PersianHub.API.Services.Layer3Network;

public sealed class InviteService(ApplicationDbContext db, IDateTimeProvider clock) : IInviteService
{
    public async Task<Result<InviteDto>> CreateAsync(CreateInviteDto request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.InviteeEmail) && string.IsNullOrWhiteSpace(request.InviteePhoneNumber))
            return Result<InviteDto>.Failure("At least one of InviteeEmail or InviteePhoneNumber must be provided.", ErrorCodes.ValidationFailed);

        var inviterExists = await db.AppUsers.AnyAsync(u => u.Id == request.InviterUserId, ct);
        if (!inviterExists)
            return Result<InviteDto>.Failure($"User with id {request.InviterUserId} not found.", ErrorCodes.NotFound);

        var now = clock.UtcNow;
        var entity = new Invite
        {
            InviterUserId = request.InviterUserId,
            InviteeEmail = request.InviteeEmail?.Trim().ToLowerInvariant(),
            InviteePhoneNumber = request.InviteePhoneNumber?.Trim(),
            Channel = request.Channel,
            Status = InviteStatus.Sent,
            SentAtUtc = now,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        db.Invites.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<InviteDto>.Success(ToDto(entity));
    }

    public async Task<Result<InviteDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Invites.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id, ct);
        if (entity is null)
            return Result<InviteDto>.Failure($"Invite with id {id} not found.", ErrorCodes.NotFound);

        return Result<InviteDto>.Success(ToDto(entity));
    }

    public async Task<Result<IReadOnlyList<InviteListItemDto>>> GetBySenderUserIdAsync(int inviterUserId, CancellationToken ct = default)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == inviterUserId, ct);
        if (!userExists)
            return Result<IReadOnlyList<InviteListItemDto>>.Failure($"User with id {inviterUserId} not found.", ErrorCodes.NotFound);

        var items = await db.Invites
            .AsNoTracking()
            .Where(i => i.InviterUserId == inviterUserId)
            .OrderByDescending(i => i.CreatedAtUtc)
            .Select(i => new InviteListItemDto(
                i.Id, i.InviterUserId, i.InviteeEmail, i.InviteePhoneNumber, i.Channel, i.Status, i.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<InviteListItemDto>>.Success(items);
    }

    public async Task<Result<InviteDto>> UpdateStatusAsync(int id, UpdateInviteStatusDto request, CancellationToken ct = default)
    {
        var entity = await db.Invites.FirstOrDefaultAsync(i => i.Id == id, ct);
        if (entity is null)
            return Result<InviteDto>.Failure($"Invite with id {id} not found.", ErrorCodes.NotFound);

        var now = clock.UtcNow;
        entity.Status = request.Status;
        entity.UpdatedAtUtc = now;

        if (request.Status == InviteStatus.Accepted)
            entity.AcceptedAtUtc = now;

        await db.SaveChangesAsync(ct);

        return Result<InviteDto>.Success(ToDto(entity));
    }

    private static InviteDto ToDto(Invite i) => new(
        i.Id, i.InviterUserId, i.InviteeEmail, i.InviteePhoneNumber,
        i.Channel, i.Status, i.SentAtUtc, i.AcceptedAtUtc,
        i.CreatedAtUtc, i.UpdatedAtUtc);
}
