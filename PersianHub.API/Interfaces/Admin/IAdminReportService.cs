using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Layer3Network;

namespace PersianHub.API.Interfaces.Admin;

public interface IAdminReportService
{
    Task<PagedResult<AdminReportListItemDto>> GetAllAsync(
        ReportStatus? status, string? referenceType, int? referenceId, int? userId,
        int page, int pageSize, CancellationToken ct);
    Task<Result<AdminReportDetailDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result> ResolveAsync(int id, CancellationToken ct);
    Task<Result> MarkReviewedAsync(int id, CancellationToken ct);
    Task<Result> DismissAsync(int id, CancellationToken ct);
}
