using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer3Network;

namespace PersianHub.API.Interfaces.Layer3Network;

public interface IReviewService
{
    Task<Result<ReviewDto>> CreateAsync(CreateReviewDto request, CancellationToken ct = default);
    Task<Result<ReviewDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ReviewListItemDto>>> GetByBusinessIdAsync(int businessId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ReviewListItemDto>>> GetByUserIdAsync(int appUserId, CancellationToken ct = default);
    Task<Result<ReviewDto>> UpdateAsync(int id, UpdateReviewDto request, CancellationToken ct = default);
}
