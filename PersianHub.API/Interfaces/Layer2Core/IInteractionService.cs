using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer2Core;

namespace PersianHub.API.Interfaces.Layer2Core;

public interface IInteractionService
{
    Task<Result<InteractionDto>> CreateAsync(CreateInteractionDto request, CancellationToken ct = default);
    Task<Result<InteractionDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<IReadOnlyList<InteractionListItemDto>>> GetByBusinessIdAsync(int businessId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<InteractionListItemDto>>> GetByUserIdAsync(int appUserId, CancellationToken ct = default);
    Task<Result<InteractionCountsDto>> GetCountsByBusinessIdAsync(int businessId, CancellationToken ct = default);
}
