using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;

namespace PersianHub.API.Interfaces.Admin;

public interface IAdminDealService
{
    Task<PagedResult<AdminDealListItemDto>> GetAllAsync(int? businessId, bool? isPublished, string? search, int page, int pageSize, CancellationToken ct);
    Task<Result<AdminDealDetailDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<AdminDealDetailDto>> CreateAsync(AdminCreateDealDto dto, CancellationToken ct);
    Task<Result<AdminDealDetailDto>> UpdateAsync(int id, AdminUpdateDealDto dto, CancellationToken ct);
    Task<Result> TogglePublishedAsync(int id, CancellationToken ct);
    Task<Result> DeleteAsync(int id, CancellationToken ct);
}
