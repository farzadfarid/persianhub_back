using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Common;
using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.Interfaces.Admin;

public interface IAdminCommunityPostService
{
    Task<PagedResult<AdminCommunityPostListItemDto>> GetAllAsync(
        int? userId, ContentStatus? status, PostType? postType, string? search,
        int page, int pageSize, CancellationToken ct);

    Task<Result<AdminCommunityPostDetailDto>> GetByIdAsync(int id, CancellationToken ct);

    Task<Result> ApproveAsync(int id, CancellationToken ct);

    Task<Result> RejectAsync(int id, CancellationToken ct);

    Task<Result> ArchiveAsync(int id, CancellationToken ct);
}
