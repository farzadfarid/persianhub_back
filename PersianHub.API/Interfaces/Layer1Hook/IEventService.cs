using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer1Hook;

namespace PersianHub.API.Interfaces.Layer1Hook;

public interface IEventService
{
    Task<Result<EventDto>> CreateAsync(CreateEventDto request, CancellationToken ct = default);
    Task<Result<EventDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<EventDto>> GetBySlugAsync(string slug, CancellationToken ct = default);
    Task<Result<IReadOnlyList<EventListItemDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyList<EventListItemDto>>> GetPublishedAsync(CancellationToken ct = default);
    Task<Result<IReadOnlyList<EventListItemDto>>> GetByBusinessIdAsync(int businessId, CancellationToken ct = default);
    Task<Result<EventDto>> UpdateAsync(int id, UpdateEventDto request, CancellationToken ct = default);
    Task<Result> SetPublishedStatusAsync(int id, bool isPublished, CancellationToken ct = default);
    Task<Result> DeleteAsync(int id, CancellationToken ct = default);
}
