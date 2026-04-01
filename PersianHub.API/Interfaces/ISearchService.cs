using PersianHub.API.DTOs.Search;

namespace PersianHub.API.Interfaces;

public interface ISearchService
{
    Task<PagedResult<BusinessSearchItemDto>> SearchBusinessesAsync(BusinessSearchRequestDto request, CancellationToken ct = default);
    Task<PagedResult<OfferSearchItemDto>> SearchOffersAsync(OfferSearchRequestDto request, CancellationToken ct = default);
    Task<PagedResult<EventSearchItemDto>> SearchEventsAsync(EventSearchRequestDto request, CancellationToken ct = default);
}
