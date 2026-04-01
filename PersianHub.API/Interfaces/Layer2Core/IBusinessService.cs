using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer2Core;

namespace PersianHub.API.Interfaces.Layer2Core;

public interface IBusinessService
{
    Task<Result<BusinessDetailsDto>> CreateAsync(CreateBusinessRequestDto request, CancellationToken ct = default);
    Task<Result<BusinessDetailsDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<BusinessDetailsDto>> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<Result<IReadOnlyList<BusinessListItemDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyList<BusinessListItemDto>>> GetMyBusinessesAsync(CancellationToken ct = default);
    Task<Result<BusinessDetailsDto>> UpdateAsync(int id, UpdateBusinessRequestDto request, CancellationToken ct = default);
    Task<Result> SetActiveStatusAsync(int id, bool isActive, CancellationToken ct = default);
}
