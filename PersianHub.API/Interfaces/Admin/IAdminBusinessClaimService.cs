using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;

namespace PersianHub.API.Interfaces.Admin;

public interface IAdminBusinessClaimService
{
    Task<Result<IReadOnlyList<BusinessClaimListItemDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<BusinessClaimDetailDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<BusinessClaimDetailDto>> ApproveAsync(int id, CancellationToken ct = default);
    Task<Result<BusinessClaimDetailDto>> RejectAsync(int id, CancellationToken ct = default);
}
