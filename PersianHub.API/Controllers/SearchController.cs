using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Interfaces;

namespace PersianHub.API.Controllers;

/// <summary>
/// Public discovery endpoints for businesses, offers, and events.
/// All endpoints are unauthenticated — no sensitive data is exposed.
/// Ranking: IsFeatured → PrioritySubscription → AverageRating → recency.
/// </summary>
[Route("api/v1/search")]
[AllowAnonymous]
public sealed class SearchController(ISearchService searchService) : ApiControllerBase
{
    /// <summary>
    /// Searches businesses. Results are ranked by featured status, subscription tier,
    /// average rating, and recency. Max page size: 50.
    /// </summary>
    [HttpGet("businesses")]
    [ProducesResponseType(typeof(PagedResult<BusinessSearchItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchBusinesses(
        [FromQuery] string? keyword,
        [FromQuery] int? categoryId,
        [FromQuery] string? city,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var request = new BusinessSearchRequestDto(keyword, categoryId, city, page, pageSize);
        return Ok(await searchService.SearchBusinessesAsync(request, ct));
    }

    /// <summary>
    /// Searches active, published daily offers.
    /// Optionally filter by keyword, city, and price range.
    /// </summary>
    [HttpGet("offers")]
    [ProducesResponseType(typeof(PagedResult<OfferSearchItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchOffers(
        [FromQuery] string? keyword,
        [FromQuery] string? city,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var request = new OfferSearchRequestDto(keyword, city, minPrice, maxPrice, page, pageSize);
        return Ok(await searchService.SearchOffersAsync(request, ct));
    }

    /// <summary>
    /// Searches published events. Results are ordered by start date (upcoming first).
    /// Optionally filter by keyword, city, price range, or free/paid.
    /// </summary>
    [HttpGet("events")]
    [ProducesResponseType(typeof(PagedResult<EventSearchItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchEvents(
        [FromQuery] string? keyword,
        [FromQuery] string? city,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] bool? isFree,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        var request = new EventSearchRequestDto(keyword, city, minPrice, maxPrice, isFree, page, pageSize);
        return Ok(await searchService.SearchEventsAsync(request, ct));
    }
}
