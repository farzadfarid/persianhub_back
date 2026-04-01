using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Enums.Common;

namespace PersianHub.API.Interfaces.Layer1Hook;

public interface IFavoriteService
{
    Task<Result<FavoriteDto>> AddAsync(AddFavoriteDto request, CancellationToken ct = default);
    Task<Result> RemoveAsync(int id, CancellationToken ct = default);
    Task<Result<FavoriteDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<IReadOnlyList<FavoriteListItemDto>>> GetByUserIdAsync(int appUserId, CancellationToken ct = default);
    Task<Result<bool>> IsFavoritedAsync(int appUserId, ReferenceType referenceType, int referenceId, CancellationToken ct = default);
}
