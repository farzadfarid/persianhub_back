using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;

namespace PersianHub.API.Interfaces.Admin;

public interface IAdminReferralCodeService
{
    Task<PagedResult<AdminReferralCodeListItemDto>> GetAllAsync(
        int? userId, bool? isActive, string? search,
        int page, int pageSize, CancellationToken ct);

    Task<Result<AdminReferralCodeDetailDto>> GetByIdAsync(int id, CancellationToken ct);

    Task<Result<AdminReferralCodeDetailDto>> CreateAsync(AdminCreateReferralCodeDto dto, CancellationToken ct);

    Task<Result> ToggleActiveAsync(int id, CancellationToken ct);
}
