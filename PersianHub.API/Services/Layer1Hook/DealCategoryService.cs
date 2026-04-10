using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Entities.Layer1Hook;
using PersianHub.API.Interfaces.Layer1Hook;

namespace PersianHub.API.Services.Layer1Hook;

public sealed class DealCategoryService(ApplicationDbContext db) : IDealCategoryService
{
    public async Task<Result<IReadOnlyList<DealCategoryDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var categories = await db.DealCategories
            .AsNoTracking()
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new DealCategoryDto(c.Id, c.Name, c.NameFa, c.Slug, c.Description, c.DescriptionFa, c.DisplayOrder, c.IsActive))
            .ToListAsync(ct);

        return Result<IReadOnlyList<DealCategoryDto>>.Success(categories);
    }

    public async Task<Result<DealCategoryDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var category = await db.DealCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (category is null)
            return Result<DealCategoryDto>.Failure($"Deal category with id {id} not found.", ErrorCodes.NotFound);

        return Result<DealCategoryDto>.Success(ToDto(category));
    }

    public async Task<Result<DealCategoryDto>> CreateAsync(UpsertDealCategoryDto dto, CancellationToken ct = default)
    {
        var slug = dto.Slug ?? dto.Name.ToLowerInvariant().Replace(" ", "-");

        var entity = new DealCategory
        {
            Name = dto.Name.Trim(),
            NameFa = dto.NameFa?.Trim(),
            Slug = slug.Trim(),
            Description = dto.Description?.Trim(),
            DescriptionFa = dto.DescriptionFa?.Trim(),
            DisplayOrder = dto.DisplayOrder,
            IsActive = dto.IsActive
        };

        db.DealCategories.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<DealCategoryDto>.Success(ToDto(entity));
    }

    public async Task<Result<DealCategoryDto>> UpdateAsync(int id, UpsertDealCategoryDto dto, CancellationToken ct = default)
    {
        var entity = await db.DealCategories.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (entity is null)
            return Result<DealCategoryDto>.Failure($"Deal category with id {id} not found.", ErrorCodes.NotFound);

        entity.Name = dto.Name.Trim();
        entity.NameFa = dto.NameFa?.Trim();
        entity.Slug = (dto.Slug ?? dto.Name.ToLowerInvariant().Replace(" ", "-")).Trim();
        entity.Description = dto.Description?.Trim();
        entity.DescriptionFa = dto.DescriptionFa?.Trim();
        entity.DisplayOrder = dto.DisplayOrder;
        entity.IsActive = dto.IsActive;

        await db.SaveChangesAsync(ct);

        return Result<DealCategoryDto>.Success(ToDto(entity));
    }

    public async Task<Result> SetActiveStatusAsync(int id, bool isActive, CancellationToken ct = default)
    {
        var entity = await db.DealCategories.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (entity is null)
            return Result.Failure($"Deal category with id {id} not found.", ErrorCodes.NotFound);

        entity.IsActive = isActive;
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.DealCategories.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (entity is null)
            return Result.Failure($"Deal category with id {id} not found.", ErrorCodes.NotFound);

        db.DealCategories.Remove(entity);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    private static DealCategoryDto ToDto(DealCategory c) =>
        new(c.Id, c.Name, c.NameFa, c.Slug, c.Description, c.DescriptionFa, c.DisplayOrder, c.IsActive);
}
