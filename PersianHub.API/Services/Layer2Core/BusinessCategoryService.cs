using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Entities.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Services.Layer2Core;

public sealed class BusinessCategoryService(ApplicationDbContext db) : IBusinessCategoryService
{
    public async Task<Result<IReadOnlyList<BusinessCategoryDto>>> GetAllActiveAsync(CancellationToken ct = default)
    {
        var categories = await db.BusinessCategories
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new BusinessCategoryDto(c.Id, c.Name, c.NameFa, c.Slug, c.Description, c.DescriptionFa, c.DisplayOrder, c.IsActive))
            .ToListAsync(ct);

        return Result<IReadOnlyList<BusinessCategoryDto>>.Success(categories);
    }

    public async Task<Result<IReadOnlyList<BusinessCategoryDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var categories = await db.BusinessCategories
            .AsNoTracking()
            .OrderBy(c => c.DisplayOrder)
            .Select(c => new BusinessCategoryDto(c.Id, c.Name, c.NameFa, c.Slug, c.Description, c.DescriptionFa, c.DisplayOrder, c.IsActive))
            .ToListAsync(ct);

        return Result<IReadOnlyList<BusinessCategoryDto>>.Success(categories);
    }

    public async Task<Result<BusinessCategoryDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var category = await db.BusinessCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (category is null)
            return Result<BusinessCategoryDto>.Failure($"Business category with id {id} not found.", ErrorCodes.NotFound);

        return Result<BusinessCategoryDto>.Success(ToDto(category));
    }

    public async Task<Result<BusinessCategoryDto>> CreateAsync(UpsertBusinessCategoryDto dto, CancellationToken ct = default)
    {
        var slug = dto.Slug ?? dto.Name.ToLowerInvariant().Replace(" ", "-");

        var entity = new BusinessCategory
        {
            Name = dto.Name.Trim(),
            NameFa = dto.NameFa?.Trim(),
            Slug = slug.Trim(),
            Description = dto.Description?.Trim(),
            DescriptionFa = dto.DescriptionFa?.Trim(),
            DisplayOrder = dto.DisplayOrder,
            IsActive = dto.IsActive
        };

        db.BusinessCategories.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<BusinessCategoryDto>.Success(ToDto(entity));
    }

    public async Task<Result<BusinessCategoryDto>> UpdateAsync(int id, UpsertBusinessCategoryDto dto, CancellationToken ct = default)
    {
        var entity = await db.BusinessCategories.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (entity is null)
            return Result<BusinessCategoryDto>.Failure($"Business category with id {id} not found.", ErrorCodes.NotFound);

        entity.Name = dto.Name.Trim();
        entity.NameFa = dto.NameFa?.Trim();
        entity.Slug = (dto.Slug ?? dto.Name.ToLowerInvariant().Replace(" ", "-")).Trim();
        entity.Description = dto.Description?.Trim();
        entity.DescriptionFa = dto.DescriptionFa?.Trim();
        entity.DisplayOrder = dto.DisplayOrder;
        entity.IsActive = dto.IsActive;

        await db.SaveChangesAsync(ct);

        return Result<BusinessCategoryDto>.Success(ToDto(entity));
    }

    public async Task<Result> SetActiveStatusAsync(int id, bool isActive, CancellationToken ct = default)
    {
        var entity = await db.BusinessCategories.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (entity is null)
            return Result.Failure($"Business category with id {id} not found.", ErrorCodes.NotFound);

        entity.IsActive = isActive;
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.BusinessCategories
            .Include(c => c.Businesses)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (entity is null)
            return Result.Failure($"Business category with id {id} not found.", ErrorCodes.NotFound);

        if (entity.Businesses.Count > 0)
            return Result.Failure("Cannot delete a business category that has businesses linked to it.", ErrorCodes.Conflict);

        db.BusinessCategories.Remove(entity);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    private static BusinessCategoryDto ToDto(BusinessCategory c) =>
        new(c.Id, c.Name, c.NameFa, c.Slug, c.Description, c.DescriptionFa, c.DisplayOrder, c.IsActive);
}
