using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Entities.Layer1Hook;
using PersianHub.API.Enums.Layer1Hook;
using PersianHub.API.Interfaces.Layer1Hook;

namespace PersianHub.API.Services.Layer1Hook;

public sealed class DealService(ApplicationDbContext db, IDateTimeProvider clock) : IDealService
{
    public async Task<Result<DealDto>> CreateAsync(CreateDealDto request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return Result<DealDto>.Failure("Title is required.", ErrorCodes.ValidationFailed);

        var businessExists = await db.Businesses.AnyAsync(b => b.Id == request.BusinessId, ct);
        if (!businessExists)
            return Result<DealDto>.Failure($"Business with id {request.BusinessId} not found.", ErrorCodes.NotFound);

        var baseSlug = string.IsNullOrWhiteSpace(request.Slug)
            ? SlugHelper.Generate(request.Title)
            : SlugHelper.Generate(request.Slug);

        if (string.IsNullOrEmpty(baseSlug))
            return Result<DealDto>.Failure("Could not generate a valid slug.", ErrorCodes.ValidationFailed);

        var existingSlugs = await db.Deals
            .Where(d => d.Slug.StartsWith(baseSlug))
            .Select(d => d.Slug)
            .ToListAsync(ct);

        var slug = SlugHelper.MakeUnique(baseSlug, existingSlugs);
        var now = clock.UtcNow;

        var entity = new Deal
        {
            BusinessId = request.BusinessId,
            Title = request.Title.Trim(),
            TitleFa = request.TitleFa?.Trim(),
            Slug = slug,
            Description = request.Description?.Trim(),
            DescriptionFa = request.DescriptionFa?.Trim(),
            DiscountType = request.DiscountType,
            DiscountValue = request.DiscountValue,
            OriginalPrice = request.OriginalPrice,
            DiscountedPrice = request.DiscountedPrice,
            Currency = request.Currency,
            ValidFromUtc = request.ValidFromUtc,
            ValidToUtc = request.ValidToUtc,
            CouponCode = request.CouponCode?.Trim(),
            TermsAndConditions = request.TermsAndConditions?.Trim(),
            TermsAndConditionsFa = request.TermsAndConditionsFa?.Trim(),
            CoverImageUrl = request.CoverImageUrl?.Trim(),
            IsPublished = false,
            CreatedAtUtc = now,
            UpdatedAtUtc = now,
        };

        db.Deals.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<DealDto>.Success(ToDto(entity));
    }

    public async Task<Result<DealDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Deals.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id, ct);
        if (entity is null)
            return Result<DealDto>.Failure($"Deal with id {id} not found.", ErrorCodes.NotFound);

        return Result<DealDto>.Success(ToDto(entity));
    }

    public async Task<Result<DealDto>> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        var entity = await db.Deals.AsNoTracking().FirstOrDefaultAsync(d => d.Slug == slug, ct);
        if (entity is null)
            return Result<DealDto>.Failure($"Deal with slug '{slug}' not found.", ErrorCodes.NotFound);

        return Result<DealDto>.Success(ToDto(entity));
    }

    public async Task<Result<IReadOnlyList<DealListItemDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await db.Deals
            .AsNoTracking()
            .OrderByDescending(d => d.CreatedAtUtc)
            .Select(d => ToListItemDto(d))
            .ToListAsync(ct);

        return Result<IReadOnlyList<DealListItemDto>>.Success(items);
    }

    public async Task<Result<IReadOnlyList<DealListItemDto>>> GetActiveAsync(CancellationToken ct = default)
    {
        var now = clock.UtcNow;
        var items = await db.Deals
            .AsNoTracking()
            .Where(d => d.IsPublished && (d.ValidToUtc == null || d.ValidToUtc >= now))
            .OrderByDescending(d => d.CreatedAtUtc)
            .Select(d => ToListItemDto(d))
            .ToListAsync(ct);

        return Result<IReadOnlyList<DealListItemDto>>.Success(items);
    }

    public async Task<Result<IReadOnlyList<DealListItemDto>>> GetByBusinessIdAsync(int businessId, CancellationToken ct = default)
    {
        var businessExists = await db.Businesses.AnyAsync(b => b.Id == businessId, ct);
        if (!businessExists)
            return Result<IReadOnlyList<DealListItemDto>>.Failure($"Business with id {businessId} not found.", ErrorCodes.NotFound);

        var items = await db.Deals
            .AsNoTracking()
            .Where(d => d.BusinessId == businessId)
            .OrderByDescending(d => d.CreatedAtUtc)
            .Select(d => ToListItemDto(d))
            .ToListAsync(ct);

        return Result<IReadOnlyList<DealListItemDto>>.Success(items);
    }

    public async Task<Result<DealDto>> UpdateAsync(int id, UpdateDealDto request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return Result<DealDto>.Failure("Title is required.", ErrorCodes.ValidationFailed);

        var entity = await db.Deals.FirstOrDefaultAsync(d => d.Id == id, ct);
        if (entity is null)
            return Result<DealDto>.Failure($"Deal with id {id} not found.", ErrorCodes.NotFound);

        entity.Title = request.Title.Trim();
        entity.TitleFa = request.TitleFa?.Trim();
        entity.Description = request.Description?.Trim();
        entity.DescriptionFa = request.DescriptionFa?.Trim();
        entity.DiscountType = request.DiscountType;
        entity.DiscountValue = request.DiscountValue;
        entity.OriginalPrice = request.OriginalPrice;
        entity.DiscountedPrice = request.DiscountedPrice;
        entity.Currency = request.Currency;
        entity.ValidFromUtc = request.ValidFromUtc;
        entity.ValidToUtc = request.ValidToUtc;
        entity.CouponCode = request.CouponCode?.Trim();
        entity.TermsAndConditions = request.TermsAndConditions?.Trim();
        entity.TermsAndConditionsFa = request.TermsAndConditionsFa?.Trim();
        entity.CoverImageUrl = request.CoverImageUrl?.Trim();
        entity.UpdatedAtUtc = clock.UtcNow;

        await db.SaveChangesAsync(ct);

        return Result<DealDto>.Success(ToDto(entity));
    }

    public async Task<Result> SetPublishedStatusAsync(int id, bool isPublished, CancellationToken ct = default)
    {
        var entity = await db.Deals.FirstOrDefaultAsync(d => d.Id == id, ct);
        if (entity is null)
            return Result.Failure($"Deal with id {id} not found.", ErrorCodes.NotFound);

        entity.IsPublished = isPublished;
        entity.UpdatedAtUtc = clock.UtcNow;
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Deals.FirstOrDefaultAsync(d => d.Id == id, ct);
        if (entity is null)
            return Result.Failure($"Deal with id {id} not found.", ErrorCodes.NotFound);

        db.Deals.Remove(entity);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    private static DealDto ToDto(Deal d) => new(
        d.Id, d.BusinessId, d.Title, d.TitleFa, d.Slug, d.Description, d.DescriptionFa,
        d.DiscountType, d.DiscountValue, d.OriginalPrice, d.DiscountedPrice,
        d.Currency, d.ValidFromUtc, d.ValidToUtc, d.CouponCode,
        d.TermsAndConditions, d.TermsAndConditionsFa, d.CoverImageUrl, d.IsPublished,
        d.CreatedAtUtc, d.UpdatedAtUtc);

    private static DealListItemDto ToListItemDto(Deal d) => new(
        d.Id, d.BusinessId, d.Title, d.TitleFa, d.Slug,
        d.DiscountType, d.DiscountValue, d.ValidFromUtc, d.ValidToUtc,
        d.CouponCode, d.CoverImageUrl, d.IsPublished);
}
