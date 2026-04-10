using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Layer1Hook;

namespace PersianHub.API.Interfaces.Admin;

public interface IAdminEventService
{
    Task<PagedResult<AdminEventListItemDto>> GetAllAsync(int? businessId, EventStatus? status, bool? isPublished, DateTime? fromUtc, DateTime? toUtc, int page, int pageSize, CancellationToken ct);
    Task<Result<AdminEventDetailDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<AdminEventDetailDto>> CreateAsync(AdminCreateEventDto dto, CancellationToken ct);
    Task<Result<AdminEventDetailDto>> UpdateAsync(int id, AdminUpdateEventDto dto, CancellationToken ct);
    Task<Result> TogglePublishedAsync(int id, CancellationToken ct);
    Task<Result> CancelAsync(int id, CancellationToken ct);
}
