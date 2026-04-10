using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer2Core;

namespace PersianHub.API.Interfaces.Layer2Core;

public interface IContactRequestService
{
    Task<Result<IReadOnlyList<ContactRequestListItemDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<ContactRequestDto>> CreateAsync(CreateContactRequestDto request, CancellationToken ct = default);
    Task<Result<ContactRequestDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ContactRequestListItemDto>>> GetByBusinessIdAsync(int businessId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<ContactRequestListItemDto>>> GetByUserIdAsync(int appUserId, CancellationToken ct = default);
    Task<Result> MarkConvertedAsync(int id, CancellationToken ct = default);
}
