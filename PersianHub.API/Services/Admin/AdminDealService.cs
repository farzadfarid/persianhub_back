using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Entities.Layer1Hook;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

/// <summary>
/// Admin service for Deal management. Delete is implemented as de-publishing (IsPublished = false)
/// since the Deal entity does not have an IsDeleted field. A migration adding IsDeleted can be
/// used in the future for true soft-delete.
/// </summary>
public sealed class AdminDealService(ApplicationDbContext db) : IAdminDealService
{
    public async Task<PagedResult<AdminDealListItemDto>> GetAllAsync(
        int? businessId, bool? isPublished, string? search,
        int page, int pageSize, CancellationToken ct)
    {
        var query = db.Deals
            .AsNoTracking()
            .Include(d => d.Business)
            .AsQueryable();

        if (businessId.HasValue)
            query = query.Where(d => d.BusinessId == businessId.Value);

        if (isPublished.HasValue)
            query = query.Where(d => d.IsPublished == isPublished.Value);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(d => d.Title.Contains(search));

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(d => d.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(d => new AdminDealListItemDto(
                d.Id, d.BusinessId, d.Business.Name, d.Title, d.TitleFa, d.Slug,
                d.DiscountType, d.DiscountValue, d.OriginalPrice, d.DiscountedPrice,
                d.Currency, d.ValidFromUtc, d.ValidToUtc, d.CouponCode, d.IsPublished, d.CreatedAtUtc))
            .ToListAsync(ct);

        return new PagedResult<AdminDealListItemDto>(items, total, page, pageSize);
    }

    public async Task<Result<AdminDealDetailDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var deal = await db.Deals
            .AsNoTracking()
            .Include(d => d.Business)
            .FirstOrDefaultAsync(d => d.Id == id, ct);

        if (deal is null)
            return Result<AdminDealDetailDto>.Failure("Deal not found.", ErrorCodes.NotFound);

        return Result<AdminDealDetailDto>.Success(ToDetailDto(deal));
    }

    public async Task<Result<AdminDealDetailDto>> CreateAsync(AdminCreateDealDto dto, CancellationToken ct)
    {
        var business = await db.Businesses.FindAsync([dto.BusinessId], ct);
        if (business is null)
            return Result<AdminDealDetailDto>.Failure("Business not found.", ErrorCodes.NotFound);

        var deal = new Deal
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
            ValidFromUtc = dto.ValidFromUtc,
            ValidToUtc = dto.ValidToUtc,
            CouponCode = dto.CouponCode,
            TermsAndConditions = dto.TermsAndConditions,
            TermsAndConditionsFa = dto.TermsAndConditionsFa,
            CoverImageUrl = dto.CoverImageUrl,
            IsPublished = true,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
        };

        db.Deals.Add(deal);
        await db.SaveChangesAsync(ct);

        deal.Business = business;
        return Result<AdminDealDetailDto>.Success(ToDetailDto(deal));
    }

    public async Task<Result<AdminDealDetailDto>> UpdateAsync(int id, AdminUpdateDealDto dto, CancellationToken ct)
    {
        var deal = await db.Deals
            .Include(d => d.Business)
            .FirstOrDefaultAsync(d => d.Id == id, ct);

        if (deal is null)
            return Result<AdminDealDetailDto>.Failure("Deal not found.", ErrorCodes.NotFound);

        deal.Title = dto.Title;
        deal.TitleFa = dto.TitleFa;
        deal.Description = dto.Description;
        deal.DescriptionFa = dto.DescriptionFa;
        deal.DiscountType = dto.DiscountType;
        deal.DiscountValue = dto.DiscountValue;
        deal.OriginalPrice = dto.OriginalPrice;
        deal.DiscountedPrice = dto.DiscountedPrice;
        deal.Currency = dto.Currency ?? "SEK";
        deal.ValidFromUtc = dto.ValidFromUtc;
        deal.ValidToUtc = dto.ValidToUtc;
        deal.CouponCode = dto.CouponCode;
        deal.TermsAndConditions = dto.TermsAndConditions;
        deal.TermsAndConditionsFa = dto.TermsAndConditionsFa;
        deal.CoverImageUrl = dto.CoverImageUrl;
        deal.UpdatedAtUtc = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
        return Result<AdminDealDetailDto>.Success(ToDetailDto(deal));
    }

    public async Task<Result> TogglePublishedAsync(int id, CancellationToken ct)
    {
        var deal = await db.Deals.FindAsync([id], ct);
        if (deal is null)
            return Result.Failure("Deal not found.", ErrorCodes.NotFound);

        deal.IsPublished = !deal.IsPublished;
        deal.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }

    /// <summary>Soft-delete: unpublishes the deal. Add IsDeleted migration for true soft-delete.</summary>
    public async Task<Result> DeleteAsync(int id, CancellationToken ct)
    {
        var deal = await db.Deals.FindAsync([id], ct);
        if (deal is null)
            return Result.Failure("Deal not found.", ErrorCodes.NotFound);

        deal.IsPublished = false;
        deal.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }

    private static AdminDealDetailDto ToDetailDto(Deal d) => new(
        d.Id, d.BusinessId, d.Business?.Name, d.Title, d.TitleFa, d.Slug, d.Description, d.DescriptionFa,
        d.DiscountType, d.DiscountValue, d.OriginalPrice, d.DiscountedPrice,
        d.Currency, d.ValidFromUtc, d.ValidToUtc, d.CouponCode,
        d.TermsAndConditions, d.TermsAndConditionsFa, d.CoverImageUrl, d.IsPublished,
        d.CreatedAtUtc, d.UpdatedAtUtc);

    private static string GenerateSlug(string title) =>
        title.ToLowerInvariant().Replace(' ', '-').Replace("'", "");
}
