using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer1Hook;

namespace PersianHub.API.Interfaces.Layer1Hook;

public interface IDealService
{
    Task<Result<DealDto>> CreateAsync(CreateDealDto request, CancellationToken ct = default);
    Task<Result<DealDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<DealDto>> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<Result<IReadOnlyList<DealListItemDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyList<DealListItemDto>>> GetActiveAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyList<DealListItemDto>>> GetByBusinessIdAsync(int businessId, CancellationToken ct = default);
    Task<Result<DealDto>> UpdateAsync(int id, UpdateDealDto request, CancellationToken ct = default);
    Task<Result> SetPublishedStatusAsync(int id, bool isPublished, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
}
