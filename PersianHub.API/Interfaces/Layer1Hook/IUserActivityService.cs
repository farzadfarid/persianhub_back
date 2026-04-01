using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Enums.Common;

namespace PersianHub.API.Interfaces.Layer1Hook;

public interface IUserActivityService
{
    Task<Result<UserActivityDto>> CreateAsync(CreateUserActivityDto request, CancellationToken ct = default);
    Task<Result<UserActivityDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<IReadOnlyList<UserActivityListItemDto>>> GetByUserIdAsync(int appUserId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<UserActivityListItemDto>>> GetByReferenceAsync(ReferenceType referenceType, int referenceId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<UserActivityListItemDto>>> GetRecentByUserIdAsync(int appUserId, int count, CancellationToken ct = default);
}
