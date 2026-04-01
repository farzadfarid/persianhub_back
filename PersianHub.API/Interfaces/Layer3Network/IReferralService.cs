using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer3Network;

namespace PersianHub.API.Interfaces.Layer3Network;

public interface IReferralService
{
    Task<Result<ReferralDto>> CreateAsync(CreateReferralDto request, CancellationToken ct = default);
    Task<Result<ReferralDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ReferralListItemDto>>> GetByReferrerUserIdAsync(int referrerUserId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ReferralListItemDto>>> GetByReferredUserIdAsync(int referredUserId, CancellationToken ct = default);
    Task<Result<ReferralDto>> UpdateStatusAsync(int id, UpdateReferralStatusDto request, CancellationToken ct = default);
}
