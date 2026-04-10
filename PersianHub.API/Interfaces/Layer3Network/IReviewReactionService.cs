using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer3Network;

namespace PersianHub.API.Interfaces.Layer3Network;

public interface IReviewReactionService
{
    Task<Result<ReviewReactionDto>> AddAsync(CreateReviewReactionDto request, CancellationToken ct = default);
    Task<Result> RemoveAsync(int id, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ReviewReactionDto>>> GetByUserAsync(int appUserId, CancellationToken ct = default);
}
