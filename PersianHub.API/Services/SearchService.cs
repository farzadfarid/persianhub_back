using Microsoft.EntityFrameworkCore;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Layer1Hook;
using PersianHub.API.Enums.Layer2Core;
using PersianHub.API.Interfaces;

namespace PersianHub.API.Services;

/// <summary>
/// Discovery engine for businesses, offers, and events.
///
/// Ranking logic (businesses):
///   1. IsFeatured DESC          — paid visibility boost
///   2. PriorityInSearch DESC    — active subscription with PriorityInSearch plan feature
///   3. AverageRating DESC       — average of Approved reviews
///   4. CreatedAtUtc DESC        — recency
///
/// All queries use IQueryable projection — no full table loads, no N+1.
/// Page size is capped at 50.
/// </summary>
public sealed class SearchService(ApplicationDbContext db) : ISearchService
{
    private const int MaxPageSize = 50;

    public async Task<PagedResult<BusinessSearchItemDto>> SearchBusinessesAsync(
        BusinessSearchRequestDto request, CancellationToken ct = default)
    {
        var pageSize = Math.Min(Math.Max(1, request.PageSize), MaxPageSize);
        var page = Math.Max(1, request.Page);

        var query = db.Businesses.AsNoTracking().Where(b => b.IsActive);

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var kw = request.Keyword.Trim().ToLower();
            query = query.Where(b =>
                b.Name.ToLower().Contains(kw) ||
                (b.NameFa != null && b.NameFa.ToLower().Contains(kw)) ||
                (b.Description != null && b.Description.ToLower().Contains(kw)) ||
                (b.DescriptionFa != null && b.DescriptionFa.ToLower().Contains(kw)));
        }

        if (request.CategoryId.HasValue)
            query = query.Where(b => b.Categories.Any(c => c.Id == request.CategoryId.Value));

        if (!string.IsNullOrWhiteSpace(request.City))
        {
            var city = request.City.Trim().ToLower();
            query = query.Where(b => b.City != null && b.City.ToLower().Contains(city));
        }

        // Project with computed ranking fields — single SQL query.
        var projected = query.Select(b => new
        {
            b.Id, b.Name, b.NameFa, b.Slug, b.City, b.CityFa, b.PhoneNumber, b.LogoUrl, b.IsVerified, b.IsFeatured, b.CreatedAtUtc,
            AverageRating = b.Reviews
                .Where(r => r.Status == ReviewStatus.Approved)
                .Average(r => (double?)r.Rating) ?? 0.0,
            ReviewCount = b.Reviews.Count(r => r.Status == ReviewStatus.Approved),
            HasPrioritySubscription = b.Subscriptions.Any(s =>
                s.Status == SubscriptionStatus.Active &&
                s.SubscriptionPlan.PriorityInSearch)
        });

        var totalCount = await projected.CountAsync(ct);

        var items = await projected
            .OrderByDescending(x => x.IsFeatured)
            .ThenByDescending(x => x.HasPrioritySubscription)
            .ThenByDescending(x => x.AverageRating)
            .ThenByDescending(x => x.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new BusinessSearchItemDto(
                x.Id, x.Name, x.NameFa, x.Slug, x.City, x.CityFa, x.PhoneNumber, x.LogoUrl,
                x.IsVerified, x.IsFeatured, x.AverageRating, x.ReviewCount))
            .ToListAsync(ct);

        return new PagedResult<BusinessSearchItemDto>(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<OfferSearchItemDto>> SearchOffersAsync(
        OfferSearchRequestDto request, CancellationToken ct = default)
    {
        var pageSize = Math.Min(Math.Max(1, request.PageSize), MaxPageSize);
        var page = Math.Max(1, request.Page);

        var query = db.DailyOffers.AsNoTracking()
            .Where(o => o.IsActive && o.IsPublished);

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var kw = request.Keyword.Trim().ToLower();
            query = query.Where(o =>
                o.Title.ToLower().Contains(kw) ||
                (o.TitleFa != null && o.TitleFa.ToLower().Contains(kw)) ||
                (o.Description != null && o.Description.ToLower().Contains(kw)) ||
                (o.DescriptionFa != null && o.DescriptionFa.ToLower().Contains(kw)));
        }

        if (!string.IsNullOrWhiteSpace(request.City))
        {
            var city = request.City.Trim().ToLower();
            query = query.Where(o => o.Business.City != null && o.Business.City.ToLower().Contains(city));
        }

        if (request.MinPrice.HasValue)
            query = query.Where(o => (o.DiscountedPrice ?? o.OriginalPrice) >= request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            query = query.Where(o => (o.DiscountedPrice ?? o.OriginalPrice) <= request.MaxPrice.Value);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(o => o.Business.IsFeatured)
            .ThenByDescending(o => o.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new OfferSearchItemDto(
                o.Id, o.BusinessId, o.Business.Name, o.Business.Slug,
                o.Title, o.TitleFa, o.Slug, o.OriginalPrice, o.DiscountedPrice,
                o.DiscountValue, o.Currency, o.EndsAtUtc, o.CoverImageUrl))
            .ToListAsync(ct);

        return new PagedResult<OfferSearchItemDto>(items, totalCount, page, pageSize);
    }

    public async Task<PagedResult<EventSearchItemDto>> SearchEventsAsync(
        EventSearchRequestDto request, CancellationToken ct = default)
    {
        var pageSize = Math.Min(Math.Max(1, request.PageSize), MaxPageSize);
        var page = Math.Max(1, request.Page);

        var query = db.Events.AsNoTracking()
            .Where(e => e.IsPublished && e.Status == EventStatus.Published);

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var kw = request.Keyword.Trim().ToLower();
            query = query.Where(e =>
                e.Title.ToLower().Contains(kw) ||
                (e.TitleFa != null && e.TitleFa.ToLower().Contains(kw)) ||
                (e.Description != null && e.Description.ToLower().Contains(kw)) ||
                (e.DescriptionFa != null && e.DescriptionFa.ToLower().Contains(kw)));
        }

        if (!string.IsNullOrWhiteSpace(request.City))
        {
            var city = request.City.Trim().ToLower();
            query = query.Where(e => e.City != null && e.City.ToLower().Contains(city));
        }

        if (request.IsFree.HasValue)
            query = query.Where(e => e.IsFree == request.IsFree.Value);

        if (request.MinPrice.HasValue)
            query = query.Where(e => e.IsFree == false && e.Price >= request.MinPrice.Value);

        if (request.MaxPrice.HasValue)
            query = query.Where(e => e.IsFree == false && e.Price <= request.MaxPrice.Value);

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderBy(e => e.StartsAtUtc)   // upcoming events first
            .ThenByDescending(e => e.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new EventSearchItemDto(
                e.Id, e.Title, e.TitleFa, e.Slug, e.City, e.CityFa,
                e.StartsAtUtc, e.EndsAtUtc,
                e.IsFree, e.Price, e.Currency, e.CoverImageUrl))
            .ToListAsync(ct);

        return new PagedResult<EventSearchItemDto>(items, totalCount, page, pageSize);
    }
}
