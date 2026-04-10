using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Entities.Layer2Core;
using PersianHub.API.Enums.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Services.Layer2Core;

public sealed class InteractionService(ApplicationDbContext db, IDateTimeProvider clock) : IInteractionService
{
    public async Task<Result<IReadOnlyList<InteractionListItemDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await db.Interactions
            .AsNoTracking()
            .OrderByDescending(i => i.CreatedAtUtc)
            .Select(i => new InteractionListItemDto(i.Id, i.BusinessId, i.AppUserId, i.InteractionType, i.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<InteractionListItemDto>>.Success(items);
    }

    public async Task<Result<InteractionDto>> CreateAsync(CreateInteractionDto request, CancellationToken ct = default)
    {
        var businessExists = await db.Businesses.AnyAsync(b => b.Id == request.BusinessId, ct);
        if (!businessExists)
            return Result<InteractionDto>.Failure($"Business with id {request.BusinessId} not found.", ErrorCodes.NotFound);

        if (request.AppUserId.HasValue)
        {
            var userExists = await db.AppUsers.AnyAsync(u => u.Id == request.AppUserId.Value, ct);
            if (!userExists)
                return Result<InteractionDto>.Failure($"User with id {request.AppUserId.Value} not found.", ErrorCodes.NotFound);
        }

        var entity = new Interaction
        {
            BusinessId = request.BusinessId,
            AppUserId = request.AppUserId,
            InteractionType = request.InteractionType,
            ReferenceId = request.ReferenceId,
            Metadata = request.Metadata?.Trim(),
            CreatedAtUtc = clock.UtcNow
        };

        db.Interactions.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<InteractionDto>.Success(ToDto(entity));
    }

    public async Task<Result<InteractionDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Interactions.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id, ct);
        if (entity is null)
            return Result<InteractionDto>.Failure($"Interaction with id {id} not found.", ErrorCodes.NotFound);

        return Result<InteractionDto>.Success(ToDto(entity));
    }

    public async Task<Result<IReadOnlyList<InteractionListItemDto>>> GetByBusinessIdAsync(int businessId, CancellationToken ct = default)
    {
        var businessExists = await db.Businesses.AnyAsync(b => b.Id == businessId, ct);
        if (!businessExists)
            return Result<IReadOnlyList<InteractionListItemDto>>.Failure($"Business with id {businessId} not found.", ErrorCodes.NotFound);

        var items = await db.Interactions
            .AsNoTracking()
            .Where(i => i.BusinessId == businessId)
            .OrderByDescending(i => i.CreatedAtUtc)
            .Select(i => new InteractionListItemDto(i.Id, i.BusinessId, i.AppUserId, i.InteractionType, i.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<InteractionListItemDto>>.Success(items);
    }

    public async Task<Result<IReadOnlyList<InteractionListItemDto>>> GetByUserIdAsync(int appUserId, CancellationToken ct = default)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == appUserId, ct);
        if (!userExists)
            return Result<IReadOnlyList<InteractionListItemDto>>.Failure($"User with id {appUserId} not found.", ErrorCodes.NotFound);

        var items = await db.Interactions
            .AsNoTracking()
            .Where(i => i.AppUserId == appUserId)
            .OrderByDescending(i => i.CreatedAtUtc)
            .Select(i => new InteractionListItemDto(i.Id, i.BusinessId, i.AppUserId, i.InteractionType, i.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<InteractionListItemDto>>.Success(items);
    }

    public async Task<Result<InteractionCountsDto>> GetCountsByBusinessIdAsync(int businessId, CancellationToken ct = default)
    {
        var businessExists = await db.Businesses.AnyAsync(b => b.Id == businessId, ct);
        if (!businessExists)
            return Result<InteractionCountsDto>.Failure($"Business with id {businessId} not found.", ErrorCodes.NotFound);

        var counts = await db.Interactions
            .AsNoTracking()
            .Where(i => i.BusinessId == businessId)
            .GroupBy(i => i.BusinessId)
            .Select(g => new
            {
                TotalViews = g.Count(i => i.InteractionType == InteractionType.ViewBusiness),
                TotalClicks = g.Count(i =>
                    i.InteractionType == InteractionType.ClickPhone ||
                    i.InteractionType == InteractionType.ClickWebsite ||
                    i.InteractionType == InteractionType.ClickWhatsApp),
                TotalContactEvents = g.Count(i =>
                    i.InteractionType == InteractionType.ContactStarted ||
                    i.InteractionType == InteractionType.ContactCompleted)
            })
            .FirstOrDefaultAsync(ct);

        var dto = new InteractionCountsDto(
            businessId,
            counts?.TotalViews ?? 0,
            counts?.TotalClicks ?? 0,
            counts?.TotalContactEvents ?? 0);

        return Result<InteractionCountsDto>.Success(dto);
    }

    private static InteractionDto ToDto(Interaction i) => new(
        i.Id, i.BusinessId, i.AppUserId, i.InteractionType, i.ReferenceId, i.Metadata, i.CreatedAtUtc);
}
