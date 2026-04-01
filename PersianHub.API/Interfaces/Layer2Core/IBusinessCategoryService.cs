using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer2Core;

namespace PersianHub.API.Interfaces.Layer2Core;

public interface IBusinessCategoryService
{
    Task<Result<IReadOnlyList<BusinessCategoryDto>>> GetAllActiveAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyList<BusinessCategoryDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<BusinessCategoryDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<BusinessCategoryDto>> CreateAsync(UpsertBusinessCategoryDto dto, CancellationToken ct = default);
    Task<Result<BusinessCategoryDto>> UpdateAsync(int id, UpsertBusinessCategoryDto dto, CancellationToken ct = default);
    Task<Result> SetActiveStatusAsync(int id, bool isActive, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
}
