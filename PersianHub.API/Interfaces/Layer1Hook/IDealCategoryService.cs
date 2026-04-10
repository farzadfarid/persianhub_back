using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer1Hook;

namespace PersianHub.API.Interfaces.Layer1Hook;

public interface IDealCategoryService
{
    Task<Result<IReadOnlyList<DealCategoryDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<DealCategoryDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<DealCategoryDto>> CreateAsync(UpsertDealCategoryDto dto, CancellationToken ct = default);
    Task<Result<DealCategoryDto>> UpdateAsync(int id, UpsertDealCategoryDto dto, CancellationToken ct = default);
    Task<Result> SetActiveStatusAsync(int id, bool isActive, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
}
