using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Entities.Layer1Hook;
using PersianHub.API.Interfaces.Layer1Hook;

namespace PersianHub.API.Services.Layer1Hook;

public sealed class DailyOfferService(ApplicationDbContext db, IDateTimeProvider clock) : IDailyOfferService
{
    public async Task<Result<DailyOfferDto>> CreateAsync(CreateDailyOfferDto request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return Result<DailyOfferDto>.Failure("Title is required.", ErrorCodes.ValidationFailed);

        if (request.EndsAtUtc <= request.StartsAtUtc)
            return Result<DailyOfferDto>.Failure("End date must be after start date.", ErrorCodes.ValidationFailed);

        if (request.DiscountValue <= 0)
            return Result<DailyOfferDto>.Failure("Discount value must be greater than zero.", ErrorCodes.ValidationFailed);

        var businessExists = await db.Businesses.AnyAsync(b => b.Id == request.BusinessId, ct);
        if (!businessExists)
            return Result<DailyOfferDto>.Failure($"Business with id {request.BusinessId} not found.", ErrorCodes.NotFound);

        var baseSlug = string.IsNullOrWhiteSpace(request.Slug)
            ? SlugHelper.Generate(request.Title)
            : SlugHelper.Generate(request.Slug);

        if (string.IsNullOrEmpty(baseSlug))
            return Result<DailyOfferDto>.Failure("Could not generate a valid slug.", ErrorCodes.ValidationFailed);

        var existingSlugs = await db.DailyOffers
            .Where(d => d.Slug.StartsWith(baseSlug))
            .Select(d => d.Slug)
            .ToListAsync(ct);

        var slug = SlugHelper.MakeUnique(baseSlug, existingSlugs);

        var now = clock.UtcNow;
        var entity = new DailyOffer
        {
            BusinessId = request.BusinessId,
            Title = request.Title.Trim(),
            Slug = slug,
            Description = request.Description?.Trim(),
            DiscountType = request.DiscountType,
            DiscountValue = request.DiscountValue,
            OriginalPrice = request.OriginalPrice,
            DiscountedPrice = request.DiscountedPrice,
            Currency = string.IsNullOrWhiteSpace(request.Currency) ? "SEK" : request.Currency.ToUpperInvariant(),
            StartsAtUtc = request.StartsAtUtc,
            EndsAtUtc = request.EndsAtUtc,
            IsActive = false,
            IsPublished = false,
            CoverImageUrl = request.CoverImageUrl?.Trim(),
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        db.DailyOffers.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<DailyOfferDto>.Success(ToDto(entity));
    }

    public async Task<Result<DailyOfferDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.DailyOffers.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id, ct);
        if (entity is null)
            return Result<DailyOfferDto>.Failure($"DailyOffer with id {id} not found.", ErrorCodes.NotFound);

        return Result<DailyOfferDto>.Success(ToDto(entity));
    }

    public async Task<Result<DailyOfferDto>> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        var entity = await db.DailyOffers.AsNoTracking().FirstOrDefaultAsync(d => d.Slug == slug, ct);
        if (entity is null)
            return Result<DailyOfferDto>.Failure($"DailyOffer with slug '{slug}' not found.", ErrorCodes.NotFound);

        return Result<DailyOfferDto>.Success(ToDto(entity));
    }

    public async Task<Result<IReadOnlyList<DailyOfferListItemDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await db.DailyOffers
            .AsNoTracking()
            .OrderByDescending(d => d.StartsAtUtc)
            .Select(d => ToListItemDto(d))
            .ToListAsync(ct);

        return Result<IReadOnlyList<DailyOfferListItemDto>>.Success(items);
    }

    public async Task<Result<IReadOnlyList<DailyOfferListItemDto>>> GetActiveAsync(CancellationToken ct = default)
    {
        var now = clock.UtcNow;
        var items = await db.DailyOffers
            .AsNoTracking()
            .Where(d => d.IsActive && d.IsPublished && d.StartsAtUtc <= now && d.EndsAtUtc >= now)
            .OrderBy(d => d.EndsAtUtc)
            .Select(d => ToListItemDto(d))
            .ToListAsync(ct);

        return Result<IReadOnlyList<DailyOfferListItemDto>>.Success(items);
    }

    public async Task<Result<IReadOnlyList<DailyOfferListItemDto>>> GetByBusinessIdAsync(int businessId, CancellationToken ct = default)
    {
        var businessExists = await db.Businesses.AnyAsync(b => b.Id == businessId, ct);
        if (!businessExists)
            return Result<IReadOnlyList<DailyOfferListItemDto>>.Failure($"Business with id {businessId} not found.", ErrorCodes.NotFound);

        var items = await db.DailyOffers
            .AsNoTracking()
            .Where(d => d.BusinessId == businessId)
            .OrderByDescending(d => d.StartsAtUtc)
            .Select(d => ToListItemDto(d))
            .ToListAsync(ct);

        return Result<IReadOnlyList<DailyOfferListItemDto>>.Success(items);
    }

    public async Task<Result<DailyOfferDto>> UpdateAsync(int id, UpdateDailyOfferDto request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return Result<DailyOfferDto>.Failure("Title is required.", ErrorCodes.ValidationFailed);

        if (request.EndsAtUtc <= request.StartsAtUtc)
            return Result<DailyOfferDto>.Failure("End date must be after start date.", ErrorCodes.ValidationFailed);

        if (request.DiscountValue <= 0)
            return Result<DailyOfferDto>.Failure("Discount value must be greater than zero.", ErrorCodes.ValidationFailed);

        var entity = await db.DailyOffers.FirstOrDefaultAsync(d => d.Id == id, ct);
        if (entity is null)
            return Result<DailyOfferDto>.Failure($"DailyOffer with id {id} not found.", ErrorCodes.NotFound);

        entity.Title = request.Title.Trim();
        entity.Description = request.Description?.Trim();
        entity.DiscountType = request.DiscountType;
        entity.DiscountValue = request.DiscountValue;
        entity.OriginalPrice = request.OriginalPrice;
        entity.DiscountedPrice = request.DiscountedPrice;
        entity.Currency = string.IsNullOrWhiteSpace(request.Currency) ? "SEK" : request.Currency.ToUpperInvariant();
        entity.StartsAtUtc = request.StartsAtUtc;
        entity.EndsAtUtc = request.EndsAtUtc;
        entity.CoverImageUrl = request.CoverImageUrl?.Trim();
        entity.UpdatedAtUtc = clock.UtcNow;

        await db.SaveChangesAsync(ct);

        return Result<DailyOfferDto>.Success(ToDto(entity));
    }

    public async Task<Result> SetActiveStatusAsync(int id, bool isActive, CancellationToken ct = default)
    {
        var entity = await db.DailyOffers.FirstOrDefaultAsync(d => d.Id == id, ct);
        if (entity is null)
            return Result.Failure($"DailyOffer with id {id} not found.", ErrorCodes.NotFound);

        entity.IsActive = isActive;
        entity.UpdatedAtUtc = clock.UtcNow;
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> SetPublishedStatusAsync(int id, bool isPublished, CancellationToken ct = default)
    {
        var entity = await db.DailyOffers.FirstOrDefaultAsync(d => d.Id == id, ct);
        if (entity is null)
            return Result.Failure($"DailyOffer with id {id} not found.", ErrorCodes.NotFound);

        entity.IsPublished = isPublished;
        entity.UpdatedAtUtc = clock.UtcNow;
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.DailyOffers.FirstOrDefaultAsync(d => d.Id == id, ct);
        if (entity is null)
            return Result.Failure($"Daily offer with id {id} not found.", ErrorCodes.NotFound);

        db.DailyOffers.Remove(entity);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    private static DailyOfferDto ToDto(DailyOffer d) => new(
        d.Id, d.BusinessId, d.Title, d.Slug, d.Description, d.DiscountType,
        d.DiscountValue, d.OriginalPrice, d.DiscountedPrice, d.Currency,
        d.StartsAtUtc, d.EndsAtUtc, d.IsActive, d.IsPublished, d.CoverImageUrl,
        d.CreatedAtUtc, d.UpdatedAtUtc);

    private static DailyOfferListItemDto ToListItemDto(DailyOffer d) => new(
        d.Id, d.BusinessId, d.Title, d.Slug, d.DiscountType, d.DiscountValue,
        d.StartsAtUtc, d.EndsAtUtc, d.IsActive, d.IsPublished, d.CoverImageUrl);
}
