using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;

namespace PersianHub.API.Interfaces.Admin;

public interface IAdminReviewService
{
    Task<PagedResult<AdminReviewListItemDto>> GetAllAsync(int? businessId, int? userId, int? rating, int page, int pageSize, CancellationToken ct);
    Task<Result<AdminReviewDetailDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result> ApproveAsync(int id, CancellationToken ct);
    Task<Result> RejectAsync(int id, CancellationToken ct);
}
