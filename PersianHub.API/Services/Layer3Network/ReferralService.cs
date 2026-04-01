using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Entities.Layer3Network;
using PersianHub.API.Interfaces.Layer3Network;

namespace PersianHub.API.Services.Layer3Network;

public sealed class ReferralService(ApplicationDbContext db, IDateTimeProvider clock) : IReferralService
{
    public async Task<Result<ReferralDto>> CreateAsync(CreateReferralDto request, CancellationToken ct = default)
    {
        var referrerExists = await db.AppUsers.AnyAsync(u => u.Id == request.ReferrerUserId, ct);
        if (!referrerExists)
            return Result<ReferralDto>.Failure($"Referrer user with id {request.ReferrerUserId} not found.", ErrorCodes.NotFound);

        if (request.ReferredUserId.HasValue)
        {
            if (request.ReferredUserId.Value == request.ReferrerUserId)
                return Result<ReferralDto>.Failure("A user cannot refer themselves.", ErrorCodes.ValidationFailed);

            var referredExists = await db.AppUsers.AnyAsync(u => u.Id == request.ReferredUserId.Value, ct);
            if (!referredExists)
                return Result<ReferralDto>.Failure($"Referred user with id {request.ReferredUserId.Value} not found.", ErrorCodes.NotFound);
        }

        if (request.ReferralCodeId.HasValue)
        {
            var codeExists = await db.ReferralCodes.AnyAsync(r => r.Id == request.ReferralCodeId.Value && r.IsActive, ct);
            if (!codeExists)
                return Result<ReferralDto>.Failure($"ReferralCode with id {request.ReferralCodeId.Value} not found or inactive.", ErrorCodes.NotFound);
        }

        var now = clock.UtcNow;
        var entity = new Referral
        {
            ReferrerUserId = request.ReferrerUserId,
            ReferredUserId = request.ReferredUserId,
            ReferralCodeId = request.ReferralCodeId,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        db.Referrals.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<ReferralDto>.Success(ToDto(entity));
    }

    public async Task<Result<ReferralDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Referrals.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id, ct);
        if (entity is null)
            return Result<ReferralDto>.Failure($"Referral with id {id} not found.", ErrorCodes.NotFound);

        return Result<ReferralDto>.Success(ToDto(entity));
    }

    public async Task<Result<IReadOnlyList<ReferralListItemDto>>> GetByReferrerUserIdAsync(int referrerUserId, CancellationToken ct = default)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == referrerUserId, ct);
        if (!userExists)
            return Result<IReadOnlyList<ReferralListItemDto>>.Failure($"User with id {referrerUserId} not found.", ErrorCodes.NotFound);

        var items = await db.Referrals
            .AsNoTracking()
            .Where(r => r.ReferrerUserId == referrerUserId)
            .OrderByDescending(r => r.CreatedAtUtc)
            .Select(r => new ReferralListItemDto(r.Id, r.ReferrerUserId, r.ReferredUserId, r.Status, r.RewardStatus, r.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<ReferralListItemDto>>.Success(items);
    }

    public async Task<Result<IReadOnlyList<ReferralListItemDto>>> GetByReferredUserIdAsync(int referredUserId, CancellationToken ct = default)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == referredUserId, ct);
        if (!userExists)
            return Result<IReadOnlyList<ReferralListItemDto>>.Failure($"User with id {referredUserId} not found.", ErrorCodes.NotFound);

        var items = await db.Referrals
            .AsNoTracking()
            .Where(r => r.ReferredUserId == referredUserId)
            .OrderByDescending(r => r.CreatedAtUtc)
            .Select(r => new ReferralListItemDto(r.Id, r.ReferrerUserId, r.ReferredUserId, r.Status, r.RewardStatus, r.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<ReferralListItemDto>>.Success(items);
    }

    public async Task<Result<ReferralDto>> UpdateStatusAsync(int id, UpdateReferralStatusDto request, CancellationToken ct = default)
    {
        var entity = await db.Referrals.FirstOrDefaultAsync(r => r.Id == id, ct);
        if (entity is null)
            return Result<ReferralDto>.Failure($"Referral with id {id} not found.", ErrorCodes.NotFound);

        var now = clock.UtcNow;
        entity.Status = request.Status;
        entity.RewardStatus = request.RewardStatus;
        entity.UpdatedAtUtc = now;

        if (request.Status == Enums.Layer3Network.ReferralStatus.Completed)
            entity.CompletedAtUtc = now;

        await db.SaveChangesAsync(ct);

        return Result<ReferralDto>.Success(ToDto(entity));
    }

    private static ReferralDto ToDto(Referral r) => new(
        r.Id, r.ReferrerUserId, r.ReferredUserId, r.ReferralCodeId,
        r.Status, r.RewardStatus, r.CreatedAtUtc, r.CompletedAtUtc, r.UpdatedAtUtc);
}
