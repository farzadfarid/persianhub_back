using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Entities.Layer1Hook;
using PersianHub.API.Interfaces.Layer1Hook;

namespace PersianHub.API.Services.Layer1Hook;

public sealed class EventCategoryService(ApplicationDbContext db) : IEventCategoryService
{
    public async Task<Result<IReadOnlyList<EventCategoryDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var categories = await db.EventCategories
            .AsNoTracking()
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new EventCategoryDto(c.Id, c.Name, c.NameFa, c.Slug, c.Description, c.DescriptionFa, c.DisplayOrder, c.IsActive))
            .ToListAsync(ct);

        return Result<IReadOnlyList<EventCategoryDto>>.Success(categories);
    }

    public async Task<Result<EventCategoryDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var category = await db.EventCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (category is null)
            return Result<EventCategoryDto>.Failure($"Event category with id {id} not found.", ErrorCodes.NotFound);

        return Result<EventCategoryDto>.Success(ToDto(category));
    }

    public async Task<Result<EventCategoryDto>> CreateAsync(UpsertEventCategoryDto dto, CancellationToken ct = default)
    {
        var slug = dto.Slug ?? dto.Name.ToLowerInvariant().Replace(" ", "-");

        var entity = new EventCategory
        {
            Name = dto.Name.Trim(),
            NameFa = dto.NameFa?.Trim(),
            Slug = slug.Trim(),
            Description = dto.Description?.Trim(),
            DescriptionFa = dto.DescriptionFa?.Trim(),
            DisplayOrder = dto.DisplayOrder,
            IsActive = dto.IsActive
        };

        db.EventCategories.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<EventCategoryDto>.Success(ToDto(entity));
    }

    public async Task<Result<EventCategoryDto>> UpdateAsync(int id, UpsertEventCategoryDto dto, CancellationToken ct = default)
    {
        var entity = await db.EventCategories.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (entity is null)
            return Result<EventCategoryDto>.Failure($"Event category with id {id} not found.", ErrorCodes.NotFound);

        entity.Name = dto.Name.Trim();
        entity.NameFa = dto.NameFa?.Trim();
        entity.Slug = (dto.Slug ?? dto.Name.ToLowerInvariant().Replace(" ", "-")).Trim();
        entity.Description = dto.Description?.Trim();
        entity.DescriptionFa = dto.DescriptionFa?.Trim();
        entity.DisplayOrder = dto.DisplayOrder;
        entity.IsActive = dto.IsActive;

        await db.SaveChangesAsync(ct);

        return Result<EventCategoryDto>.Success(ToDto(entity));
    }

    public async Task<Result> SetActiveStatusAsync(int id, bool isActive, CancellationToken ct = default)
    {
        var entity = await db.EventCategories.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (entity is null)
            return Result.Failure($"Event category with id {id} not found.", ErrorCodes.NotFound);

        entity.IsActive = isActive;
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.EventCategories.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (entity is null)
            return Result.Failure($"Event category with id {id} not found.", ErrorCodes.NotFound);

        db.EventCategories.Remove(entity);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    private static EventCategoryDto ToDto(EventCategory c) =>
        new(c.Id, c.Name, c.NameFa, c.Slug, c.Description, c.DescriptionFa, c.DisplayOrder, c.IsActive);
}
