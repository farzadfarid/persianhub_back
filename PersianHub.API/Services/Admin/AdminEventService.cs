using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Entities.Layer1Hook;
using PersianHub.API.Enums.Layer1Hook;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

public sealed class AdminEventService(ApplicationDbContext db) : IAdminEventService
{
    public async Task<PagedResult<AdminEventListItemDto>> GetAllAsync(
        int? businessId, EventStatus? status, bool? isPublished,
        DateTime? fromUtc, DateTime? toUtc,
        int page, int pageSize, CancellationToken ct)
    {
        var query = db.Events
            .AsNoTracking()
            .Include(e => e.Business)
            .AsQueryable();

        if (businessId.HasValue)
            query = query.Where(e => e.BusinessId == businessId.Value);

        if (status.HasValue)
            query = query.Where(e => e.Status == status.Value);

        if (isPublished.HasValue)
            query = query.Where(e => e.IsPublished == isPublished.Value);

        if (fromUtc.HasValue)
            query = query.Where(e => e.StartsAtUtc >= fromUtc.Value);

        if (toUtc.HasValue)
            query = query.Where(e => e.StartsAtUtc <= toUtc.Value);

        var total = await query.CountAsync(ct);

        var items = await query
            .OrderByDescending(e => e.StartsAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new AdminEventListItemDto(
                e.Id, e.Title, e.TitleFa, e.Slug, e.City, e.CityFa, e.StartsAtUtc, e.EndsAtUtc,
                e.BusinessId, e.Business != null ? e.Business.Name : null,
                e.IsFree, e.Price, e.Currency, e.Status, e.IsPublished,
                e.CoverImageUrl, e.CreatedAtUtc))
            .ToListAsync(ct);

        return new PagedResult<AdminEventListItemDto>(items, total, page, pageSize);
    }

    public async Task<Result<AdminEventDetailDto>> GetByIdAsync(int id, CancellationToken ct)
    {
        var ev = await db.Events
            .AsNoTracking()
            .Include(e => e.Business)
            .FirstOrDefaultAsync(e => e.Id == id, ct);

        if (ev is null)
            return Result<AdminEventDetailDto>.Failure("Event not found.", ErrorCodes.NotFound);

        return Result<AdminEventDetailDto>.Success(ToDetailDto(ev));
    }

    public async Task<Result<AdminEventDetailDto>> CreateAsync(AdminCreateEventDto dto, CancellationToken ct)
    {
        var ev = new Event
        {
            Title = dto.Title,
            TitleFa = dto.TitleFa,
            Slug = string.IsNullOrWhiteSpace(dto.Slug) ? GenerateSlug(dto.Title) : dto.Slug,
            Description = dto.Description,
            DescriptionFa = dto.DescriptionFa,
            LocationName = dto.LocationName,
            LocationNameFa = dto.LocationNameFa,
            AddressLine = dto.AddressLine,
            AddressLineFa = dto.AddressLineFa,
            City = dto.City,
            CityFa = dto.CityFa,
            Region = dto.Region,
            RegionFa = dto.RegionFa,
            PostalCode = dto.PostalCode,
            Country = dto.Country,
            StartsAtUtc = dto.StartsAtUtc,
            EndsAtUtc = dto.EndsAtUtc,
            BusinessId = dto.BusinessId,
            OrganizerName = dto.OrganizerName,
            OrganizerNameFa = dto.OrganizerNameFa,
            OrganizerPhoneNumber = dto.OrganizerPhoneNumber,
            OrganizerEmail = dto.OrganizerEmail,
            CoverImageUrl = dto.CoverImageUrl,
            IsFree = dto.IsFree,
            Price = dto.Price,
            Currency = dto.Currency,
            CreatedByUserId = dto.CreatedByUserId,
            Status = EventStatus.Published,
            IsPublished = true,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow,
        };

        db.Events.Add(ev);
        await db.SaveChangesAsync(ct);

        if (ev.BusinessId.HasValue)
            ev.Business = await db.Businesses.FindAsync([ev.BusinessId.Value], ct);

        return Result<AdminEventDetailDto>.Success(ToDetailDto(ev));
    }

    public async Task<Result<AdminEventDetailDto>> UpdateAsync(int id, AdminUpdateEventDto dto, CancellationToken ct)
    {
        var ev = await db.Events
            .Include(e => e.Business)
            .FirstOrDefaultAsync(e => e.Id == id, ct);

        if (ev is null)
            return Result<AdminEventDetailDto>.Failure("Event not found.", ErrorCodes.NotFound);

        ev.Title = dto.Title;
        ev.TitleFa = dto.TitleFa;
        ev.Description = dto.Description;
        ev.DescriptionFa = dto.DescriptionFa;
        ev.LocationName = dto.LocationName;
        ev.LocationNameFa = dto.LocationNameFa;
        ev.AddressLine = dto.AddressLine;
        ev.AddressLineFa = dto.AddressLineFa;
        ev.City = dto.City;
        ev.CityFa = dto.CityFa;
        ev.Region = dto.Region;
        ev.RegionFa = dto.RegionFa;
        ev.PostalCode = dto.PostalCode;
        ev.Country = dto.Country;
        ev.StartsAtUtc = dto.StartsAtUtc;
        ev.EndsAtUtc = dto.EndsAtUtc;
        ev.BusinessId = dto.BusinessId;
        ev.OrganizerName = dto.OrganizerName;
        ev.OrganizerNameFa = dto.OrganizerNameFa;
        ev.OrganizerPhoneNumber = dto.OrganizerPhoneNumber;
        ev.OrganizerEmail = dto.OrganizerEmail;
        ev.CoverImageUrl = dto.CoverImageUrl;
        ev.IsFree = dto.IsFree;
        ev.Price = dto.Price;
        ev.Currency = dto.Currency;
        ev.UpdatedAtUtc = DateTime.UtcNow;

        await db.SaveChangesAsync(ct);
        return Result<AdminEventDetailDto>.Success(ToDetailDto(ev));
    }

    public async Task<Result> TogglePublishedAsync(int id, CancellationToken ct)
    {
        var ev = await db.Events.FindAsync([id], ct);
        if (ev is null)
            return Result.Failure("Event not found.", ErrorCodes.NotFound);

        ev.IsPublished = !ev.IsPublished;
        ev.Status = ev.IsPublished ? EventStatus.Published : EventStatus.Draft;
        ev.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }

    /// <summary>Soft-delete: sets event status to Cancelled.</summary>
    public async Task<Result> CancelAsync(int id, CancellationToken ct)
    {
        var ev = await db.Events.FindAsync([id], ct);
        if (ev is null)
            return Result.Failure("Event not found.", ErrorCodes.NotFound);

        ev.Status = EventStatus.Cancelled;
        ev.IsPublished = false;
        ev.UpdatedAtUtc = DateTime.UtcNow;
        await db.SaveChangesAsync(ct);
        return Result.Success();
    }

    private static AdminEventDetailDto ToDetailDto(Event e) => new(
        e.Id, e.Title, e.TitleFa, e.Slug, e.Description, e.DescriptionFa,
        e.LocationName, e.LocationNameFa, e.AddressLine, e.AddressLineFa,
        e.City, e.CityFa, e.Region, e.RegionFa, e.PostalCode, e.Country,
        e.StartsAtUtc, e.EndsAtUtc, e.BusinessId, e.Business?.Name,
        e.OrganizerName, e.OrganizerNameFa, e.OrganizerPhoneNumber,
        e.OrganizerEmail, e.CoverImageUrl, e.IsFree, e.Price, e.Currency,
        e.Status, e.IsPublished, e.CreatedByUserId, e.CreatedAtUtc, e.UpdatedAtUtc);

    private static string GenerateSlug(string title) =>
        title.ToLowerInvariant().Replace(' ', '-').Replace("'", "");
}
