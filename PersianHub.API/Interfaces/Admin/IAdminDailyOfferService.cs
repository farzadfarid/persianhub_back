using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;

namespace PersianHub.API.Interfaces.Admin;

public interface IAdminDailyOfferService
{
    Task<PagedResult<AdminDailyOfferListItemDto>> GetAllAsync(int? businessId, bool? isActive, int page, int pageSize, CancellationToken ct);
    Task<Result<AdminDailyOfferDetailDto>> GetByIdAsync(int id, CancellationToken ct);
    Task<Result<AdminDailyOfferDetailDto>> CreateAsync(AdminCreateDailyOfferDto dto, CancellationToken ct);
    Task<Result<AdminDailyOfferDetailDto>> UpdateAsync(int id, AdminUpdateDailyOfferDto dto, CancellationToken ct);
    Task<Result> ToggleActiveAsync(int id, CancellationToken ct);
    Task<Result> DeleteAsync(int id, CancellationToken ct);
}
