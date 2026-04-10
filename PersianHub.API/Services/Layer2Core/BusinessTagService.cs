using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Entities.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Services.Layer2Core;

public sealed class BusinessTagService(ApplicationDbContext db) : IBusinessTagService
{
    public async Task<Result<IReadOnlyList<BusinessTagDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var tags = await db.BusinessTags
            .AsNoTracking()
            .OrderBy(t => t.Name)
            .Select(t => new BusinessTagDto(t.Id, t.Name, t.NameFa, t.Slug, t.IsActive))
            .ToListAsync(ct);

        return Result<IReadOnlyList<BusinessTagDto>>.Success(tags);
    }

    public async Task<Result<BusinessTagDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var tag = await db.BusinessTags
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, ct);

        if (tag is null)
            return Result<BusinessTagDto>.Failure($"Business tag with id {id} not found.", ErrorCodes.NotFound);

        return Result<BusinessTagDto>.Success(ToDto(tag));
    }

    public async Task<Result<BusinessTagDto>> CreateAsync(UpsertBusinessTagDto dto, CancellationToken ct = default)
    {
        var slug = dto.Slug ?? dto.Name.ToLowerInvariant().Replace(" ", "-");

        var entity = new BusinessTag
        {
            Name = dto.Name.Trim(),
            NameFa = dto.NameFa?.Trim(),
            Slug = slug.Trim(),
            IsActive = dto.IsActive
        };

        db.BusinessTags.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<BusinessTagDto>.Success(ToDto(entity));
    }

    public async Task<Result<BusinessTagDto>> UpdateAsync(int id, UpsertBusinessTagDto dto, CancellationToken ct = default)
    {
        var entity = await db.BusinessTags.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (entity is null)
            return Result<BusinessTagDto>.Failure($"Business tag with id {id} not found.", ErrorCodes.NotFound);

        entity.Name = dto.Name.Trim();
        entity.NameFa = dto.NameFa?.Trim();
        entity.Slug = (dto.Slug ?? dto.Name.ToLowerInvariant().Replace(" ", "-")).Trim();
        entity.IsActive = dto.IsActive;

        await db.SaveChangesAsync(ct);

        return Result<BusinessTagDto>.Success(ToDto(entity));
    }

    public async Task<Result> SetActiveStatusAsync(int id, bool isActive, CancellationToken ct = default)
    {
        var entity = await db.BusinessTags.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (entity is null)
            return Result.Failure($"Business tag with id {id} not found.", ErrorCodes.NotFound);

        entity.IsActive = isActive;
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.BusinessTags
            .Include(t => t.Businesses)
            .FirstOrDefaultAsync(t => t.Id == id, ct);

        if (entity is null)
            return Result.Failure($"Business tag with id {id} not found.", ErrorCodes.NotFound);

        if (entity.Businesses.Count > 0)
            return Result.Failure("Cannot delete a business tag that has businesses linked to it.", ErrorCodes.Conflict);

        db.BusinessTags.Remove(entity);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    private static BusinessTagDto ToDto(BusinessTag t) => new(t.Id, t.Name, t.NameFa, t.Slug, t.IsActive);
}
