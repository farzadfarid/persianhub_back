using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer1Hook;

namespace PersianHub.API.Interfaces.Layer1Hook;

public interface IDailyOfferService
{
    Task<Result<DailyOfferDto>> CreateAsync(CreateDailyOfferDto request, CancellationToken ct = default);
    Task<Result<DailyOfferDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<DailyOfferDto>> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<Result<IReadOnlyList<DailyOfferListItemDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyList<DailyOfferListItemDto>>> GetActiveAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyList<DailyOfferListItemDto>>> GetByBusinessIdAsync(int businessId, CancellationToken ct = default);
    Task<Result<DailyOfferDto>> UpdateAsync(int id, UpdateDailyOfferDto request, CancellationToken ct = default);
    Task<Result> SetActiveStatusAsync(int id, bool isActive, CancellationToken ct = default);
    Task<Result> SetPublishedStatusAsync(int id, bool isPublished, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
}
