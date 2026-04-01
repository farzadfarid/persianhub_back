using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer2Core;

namespace PersianHub.API.Interfaces.Layer2Core;

public interface IBusinessTagService
{
    Task<Result<IReadOnlyList<BusinessTagDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<BusinessTagDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<BusinessTagDto>> CreateAsync(UpsertBusinessTagDto dto, CancellationToken ct = default);
    Task<Result<BusinessTagDto>> UpdateAsync(int id, UpsertBusinessTagDto dto, CancellationToken ct = default);
    Task<Result> SetActiveStatusAsync(int id, bool isActive, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
}
