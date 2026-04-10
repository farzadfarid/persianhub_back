using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer1Hook;

namespace PersianHub.API.Interfaces.Layer1Hook;

public interface IEventCategoryService
{
    Task<Result<IReadOnlyList<EventCategoryDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<EventCategoryDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<EventCategoryDto>> CreateAsync(UpsertEventCategoryDto dto, CancellationToken ct = default);
    Task<Result<EventCategoryDto>> UpdateAsync(int id, UpsertEventCategoryDto dto, CancellationToken ct = default);
    Task<Result> SetActiveStatusAsync(int id, bool isActive, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
}
