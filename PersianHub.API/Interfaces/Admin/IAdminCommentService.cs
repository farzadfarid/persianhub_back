using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Common;

namespace PersianHub.API.Interfaces.Admin;

public interface IAdminCommentService
{
    Task<PagedResult<AdminCommentListItemDto>> GetAllAsync(
        int? userId, ContentStatus? status, ReferenceType? referenceType, int? referenceId, string? search,
        int page, int pageSize, CancellationToken ct);

    Task<Result<AdminCommentDetailDto>> GetByIdAsync(int id, CancellationToken ct);

    Task<Result> ApproveAsync(int id, CancellationToken ct);

    Task<Result> RejectAsync(int id, CancellationToken ct);

    Task<Result> ArchiveAsync(int id, CancellationToken ct);
}
