using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Entities.Layer1Hook;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

public sealed class AdminDailyOfferService(ApplicationDbContext db) : IAdminDailyOfferService
{
    public async Task<PagedResult<AdminDailyOfferListItemDto>> GetAllAsync(
        int? businessId, bool? isActive, int page, int pageSize, CancellationToken ct)
    {
        var query = db.DailyOffers
            .AsNoTracking()
            .Include(o => o.Business)
            .AsQueryable();

        if (businessId.HasValue)
            query = query.Where(o => o.BusinessId == businessId.Value);

        if (isActive.HasValue)
            query = query.Where(o => o.IsActive == isActive.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(o => o.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new AdminDailyOfferListItemDto(
                o.Id, o.BusinessId, o.Business.Name, o.Title, o.TitleFa, o.Slug,
                o.DiscountType, o.DiscountValue, o.OriginalPrice, o.DiscountedPrice,
                o.Currency, o.StartsAtUtc, o.EndsAtUtc, o.IsActive, o.IsPublished,
                o.CoverImageUrl, o.CreatedAtUtc))
            .ToListAsync(ct);

        return new PagedResult<AdminDailyOfferListItemDto>(items, total, page, pageSize);
    }

    public async Task<Result<AdminDailyOfferDetailDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var offer = await db.DailyOffers
            .AsNoTracking()
            .Include(o => o.Business)
            .FirstOrDefaultAsync(o => o.Id == id, ct);

        if (offer is null)
            return Result<AdminDailyOfferDetailDto>.Failure("Daily offer not found.", ErrorCodes.NotFound);

        return Result<AdminDailyOfferDetailDto>.Success(ToDetailDto(offer));
    }

    public async Task<Result<AdminDailyOfferDetailDto>> CreateAsync(AdminCreateDailyOfferDto dto, CancellationToken ct)
    {
        var business = await db.Businesses.FindAsync([dto.BusinessId], ct);
        if (business is null)
            return Result<AdminDailyOfferDetailDto>.Failure("Business not found.", ErrorCodes.NotFound);

        var offer = new DailyOffer
        {
            BusinessId = dto.BusinessId,
            Title = dto.Title,
            TitleFa = dto.TitleFa,
            Slug = string.IsNullOrWhiteSpace(dto.Slug) ? GenerateSlug(dto.Title) : dto.Slug,
            Description = dto.Description,
            DescriptionFa = dto.DescriptionFa,
            DiscountType = dto.DiscountType,
            DiscountValue = dto.DiscountValue,
            OriginalPrice = dto.OriginalPrice,
            DiscountedPrice = dto.DiscountedPrice,
            Currency = dto.Currency ?? "SEK",
            StartsAtUtc = dto.StartsAtUtc,
            EndsAtUtc = dto.EndsAtUtc,
            CoverImageUrl = dto.CoverImageUrl,
            IsActive = true,
            IsPublished = true,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
        };

        db.DailyOffers.Add(offer);
        await db.SaveChangesAsync(ct);

        offer.Business = business;
        return Result<AdminDailyOfferDetailDto>.Success(ToDetailDto(offer));
    }

    public async Task<Result<AdminDailyOfferDetailDto>> UpdateAsync(int id, AdminUpdateDailyOfferDto dto, CancellationToken ct)
    {
        var offer = await db.DailyOffers
            .Include(o => o.Business)
            .FirstOrDefaultAsync(o => o.Id == id, ct);

        if (offer is null)
            return Result<AdminDailyOfferDetailDto>.Failure("Daily offer not found.", ErrorCodes.NotFound);

        offer.Title = dto.Title;
        offer.TitleFa = dto.TitleFa;
        offer.Description = dto.Description;
        offer.DescriptionFa = dto.DescriptionFa;
        offer.DiscountType = dto.DiscountType;
        offer.DiscountValue = dto.DiscountValue;
        offer.OriginalPrice = dto.OriginalPrice;
        offer.DiscountedPrice = dto.DiscountedPrice;
        offer.Currency = dto.Currency ?? "SEK";
        offer.StartsAtUtc = dto.StartsAtUtc;
        offer.EndsAtUtc = dto.EndsAtUtc;
        offer.CoverImageUrl = dto.CoverImageUrl;
        offer.UpdatedAtUtc = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
        return Result<AdminDailyOfferDetailDto>.Success(ToDetailDto(offer));
    }

    public async Task<Result> ToggleActiveAsync(int id, CancellationToken ct)
    {
        var offer = await db.DailyOffers.FindAsync([id], ct);
        if (offer is null)
            return Result.Failure("Daily offer not found.", ErrorCodes.NotFound);

        offer.IsActive = !offer.IsActive;
        offer.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct)
    {
        var offer = await db.DailyOffers.FindAsync([id], ct);
        if (offer is null)
            return Result.Failure("Daily offer not found.", ErrorCodes.NotFound);

        offer.IsActive = false;
        offer.IsPublished = false;
        offer.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }

    private static AdminDailyOfferDetailDto ToDetailDto(DailyOffer o) => new(
        o.Id, o.BusinessId, o.Business?.Name, o.Title, o.TitleFa, o.Slug, o.Description, o.DescriptionFa,
        o.DiscountType, o.DiscountValue, o.OriginalPrice, o.DiscountedPrice,
        o.Currency, o.StartsAtUtc, o.EndsAtUtc, o.IsActive, o.IsPublished,
        o.CoverImageUrl, o.CreatedAtUtc, o.UpdatedAtUtc);

    private static string GenerateSlug(string title) =>
        title.ToLowerInvariant().Replace(' ', '-').Replace("'", "");
}
