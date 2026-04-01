using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Enums.Common;

namespace PersianHub.API.Interfaces.Layer3Network;

public interface IShareLogService
{
    Task<Result<ShareLogDto>> CreateAsync(CreateShareLogDto request, CancellationToken ct = default);
    Task<Result<ShareLogDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ShareLogListItemDto>>> GetByUserIdAsync(int appUserId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ShareLogListItemDto>>> GetByReferenceAsync(ReferenceType referenceType, int referenceId, CancellationToken ct = default);
}
