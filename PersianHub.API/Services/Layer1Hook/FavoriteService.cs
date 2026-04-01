using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Entities.Layer1Hook;
using PersianHub.API.Enums.Common;
using PersianHub.API.Interfaces.Layer1Hook;

namespace PersianHub.API.Services.Layer1Hook;

public sealed class FavoriteService(ApplicationDbContext db, IDateTimeProvider clock) : IFavoriteService
{
    public async Task<Result<FavoriteDto>> AddAsync(AddFavoriteDto request, CancellationToken ct = default)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == request.AppUserId, ct);
        if (!userExists)
            return Result<FavoriteDto>.Failure($"User with id {request.AppUserId} not found.", ErrorCodes.NotFound);

        var duplicate = await db.Favorites.AnyAsync(
            f => f.AppUserId == request.AppUserId &&
                 f.ReferenceType == request.ReferenceType &&
                 f.ReferenceId == request.ReferenceId, ct);

        if (duplicate)
            return Result<FavoriteDto>.Failure("This item is already in the user's favorites.", ErrorCodes.AlreadyExists);

        var entity = new Favorite
        {
            AppUserId = request.AppUserId,
            ReferenceType = request.ReferenceType,
            ReferenceId = request.ReferenceId,
            CreatedAtUtc = clock.UtcNow
        };

        db.Favorites.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<FavoriteDto>.Success(ToDto(entity));
    }

    public async Task<Result> RemoveAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Favorites.FirstOrDefaultAsync(f => f.Id == id, ct);
        if (entity is null)
            return Result.Failure($"Favorite with id {id} not found.", ErrorCodes.NotFound);

        db.Favorites.Remove(entity);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result<FavoriteDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Favorites.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id, ct);
        if (entity is null)
            return Result<FavoriteDto>.Failure($"Favorite with id {id} not found.", ErrorCodes.NotFound);

        return Result<FavoriteDto>.Success(ToDto(entity));
    }

    public async Task<Result<IReadOnlyList<FavoriteListItemDto>>> GetByUserIdAsync(int appUserId, CancellationToken ct = default)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == appUserId, ct);
        if (!userExists)
            return Result<IReadOnlyList<FavoriteListItemDto>>.Failure($"User with id {appUserId} not found.", ErrorCodes.NotFound);

        var items = await db.Favorites
            .AsNoTracking()
            .Where(f => f.AppUserId == appUserId)
            .OrderByDescending(f => f.CreatedAtUtc)
            .Select(f => new FavoriteListItemDto(f.Id, f.ReferenceType, f.ReferenceId, f.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<FavoriteListItemDto>>.Success(items);
    }

    public async Task<Result<bool>> IsFavoritedAsync(int appUserId, ReferenceType referenceType, int referenceId, CancellationToken ct = default)
    {
        var isFavorited = await db.Favorites.AnyAsync(
            f => f.AppUserId == appUserId &&
                 f.ReferenceType == referenceType &&
                 f.ReferenceId == referenceId, ct);

        return Result<bool>.Success(isFavorited);
    }

    private static FavoriteDto ToDto(Favorite f) => new(f.Id, f.AppUserId, f.ReferenceType, f.ReferenceId, f.CreatedAtUtc);
}
