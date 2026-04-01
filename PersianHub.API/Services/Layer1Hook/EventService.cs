using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer1Hook;
using PersianHub.API.Entities.Layer1Hook;
using PersianHub.API.Enums.Layer1Hook;
using PersianHub.API.Interfaces.Layer1Hook;

namespace PersianHub.API.Services.Layer1Hook;

public sealed class EventService(ApplicationDbContext db, IDateTimeProvider clock) : IEventService
{
    public async Task<Result<EventDto>> CreateAsync(CreateEventDto request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return Result<EventDto>.Failure("Title is required.", ErrorCodes.ValidationFailed);

        if (request.EndsAtUtc.HasValue && request.EndsAtUtc.Value <= request.StartsAtUtc)
            return Result<EventDto>.Failure("End date must be after start date.", ErrorCodes.ValidationFailed);

        if (!request.IsFree && (request.Price is null || request.Price <= 0))
            return Result<EventDto>.Failure("Price must be set and greater than zero for paid events.", ErrorCodes.ValidationFailed);

        if (request.BusinessId.HasValue)
        {
            var businessExists = await db.Businesses.AnyAsync(b => b.Id == request.BusinessId.Value, ct);
            if (!businessExists)
                return Result<EventDto>.Failure($"Business with id {request.BusinessId.Value} not found.", ErrorCodes.NotFound);
        }

        if (request.CreatedByUserId.HasValue)
        {
            var userExists = await db.AppUsers.AnyAsync(u => u.Id == request.CreatedByUserId.Value, ct);
            if (!userExists)
                return Result<EventDto>.Failure($"User with id {request.CreatedByUserId.Value} not found.", ErrorCodes.NotFound);
        }

        var baseSlug = string.IsNullOrWhiteSpace(request.Slug)
            ? SlugHelper.Generate(request.Title)
            : SlugHelper.Generate(request.Slug);

        if (string.IsNullOrEmpty(baseSlug))
            return Result<EventDto>.Failure("Could not generate a valid slug.", ErrorCodes.ValidationFailed);

        var existingSlugs = await db.Events
            .Where(e => e.Slug.StartsWith(baseSlug))
            .Select(e => e.Slug)
            .ToListAsync(ct);

        var slug = SlugHelper.MakeUnique(baseSlug, existingSlugs);

        var now = clock.UtcNow;
        var entity = new Event
        {
            Title = request.Title.Trim(),
            Slug = slug,
            Description = request.Description?.Trim(),
            LocationName = request.LocationName?.Trim(),
            AddressLine = request.AddressLine?.Trim(),
            City = request.City?.Trim(),
            Region = request.Region?.Trim(),
            PostalCode = request.PostalCode?.Trim(),
            Country = request.Country?.Trim(),
            StartsAtUtc = request.StartsAtUtc,
            EndsAtUtc = request.EndsAtUtc,
            BusinessId = request.BusinessId,
            OrganizerName = request.OrganizerName?.Trim(),
            OrganizerPhoneNumber = request.OrganizerPhoneNumber?.Trim(),
            OrganizerEmail = request.OrganizerEmail?.Trim(),
            CoverImageUrl = request.CoverImageUrl?.Trim(),
            IsFree = request.IsFree,
            Price = request.IsFree ? null : request.Price,
            Currency = request.IsFree ? null : request.Currency,
            Status = EventStatus.Draft,
            IsPublished = false,
            CreatedByUserId = request.CreatedByUserId,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        db.Events.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<EventDto>.Success(ToDto(entity));
    }

    public async Task<Result<EventDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null)
            return Result<EventDto>.Failure($"Event with id {id} not found.", ErrorCodes.NotFound);

        return Result<EventDto>.Success(ToDto(entity));
    }

    public async Task<Result<EventDto>> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        var entity = await db.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Slug == slug, ct);
        if (entity is null)
            return Result<EventDto>.Failure($"Event with slug '{slug}' not found.", ErrorCodes.NotFound);

        return Result<EventDto>.Success(ToDto(entity));
    }

    public async Task<Result<IReadOnlyList<EventListItemDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await db.Events
            .AsNoTracking()
            .OrderBy(e => e.StartsAtUtc)
            .Select(e => ToListItemDto(e))
            .ToListAsync(ct);

        return Result<IReadOnlyList<EventListItemDto>>.Success(items);
    }

    public async Task<Result<IReadOnlyList<EventListItemDto>>> GetPublishedAsync(CancellationToken ct = default)
    {
        var now = clock.UtcNow;
        var items = await db.Events
            .AsNoTracking()
            .Where(e => e.IsPublished && (e.EndsAtUtc == null || e.EndsAtUtc >= now))
            .OrderBy(e => e.StartsAtUtc)
            .Select(e => ToListItemDto(e))
            .ToListAsync(ct);

        return Result<IReadOnlyList<EventListItemDto>>.Success(items);
    }

    public async Task<Result<IReadOnlyList<EventListItemDto>>> GetByBusinessIdAsync(int businessId, CancellationToken ct = default)
    {
        var businessExists = await db.Businesses.AnyAsync(b => b.Id == businessId, ct);
        if (!businessExists)
            return Result<IReadOnlyList<EventListItemDto>>.Failure($"Business with id {businessId} not found.", ErrorCodes.NotFound);

        var items = await db.Events
            .AsNoTracking()
            .Where(e => e.BusinessId == businessId)
            .OrderBy(e => e.StartsAtUtc)
            .Select(e => ToListItemDto(e))
            .ToListAsync(ct);

        return Result<IReadOnlyList<EventListItemDto>>.Success(items);
    }

    public async Task<Result<EventDto>> UpdateAsync(int id, UpdateEventDto request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return Result<EventDto>.Failure("Title is required.", ErrorCodes.ValidationFailed);

        if (request.EndsAtUtc.HasValue && request.EndsAtUtc.Value <= request.StartsAtUtc)
            return Result<EventDto>.Failure("End date must be after start date.", ErrorCodes.ValidationFailed);

        if (!request.IsFree && (request.Price is null || request.Price <= 0))
            return Result<EventDto>.Failure("Price must be set and greater than zero for paid events.", ErrorCodes.ValidationFailed);

        var entity = await db.Events.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null)
            return Result<EventDto>.Failure($"Event with id {id} not found.", ErrorCodes.NotFound);

        if (request.BusinessId.HasValue)
        {
            var businessExists = await db.Businesses.AnyAsync(b => b.Id == request.BusinessId.Value, ct);
            if (!businessExists)
                return Result<EventDto>.Failure($"Business with id {request.BusinessId.Value} not found.", ErrorCodes.NotFound);
        }

        entity.Title = request.Title.Trim();
        entity.Description = request.Description?.Trim();
        entity.LocationName = request.LocationName?.Trim();
        entity.AddressLine = request.AddressLine?.Trim();
        entity.City = request.City?.Trim();
        entity.Region = request.Region?.Trim();
        entity.PostalCode = request.PostalCode?.Trim();
        entity.Country = request.Country?.Trim();
        entity.StartsAtUtc = request.StartsAtUtc;
        entity.EndsAtUtc = request.EndsAtUtc;
        entity.BusinessId = request.BusinessId;
        entity.OrganizerName = request.OrganizerName?.Trim();
        entity.OrganizerPhoneNumber = request.OrganizerPhoneNumber?.Trim();
        entity.OrganizerEmail = request.OrganizerEmail?.Trim();
        entity.CoverImageUrl = request.CoverImageUrl?.Trim();
        entity.IsFree = request.IsFree;
        entity.Price = request.IsFree ? null : request.Price;
        entity.Currency = request.IsFree ? null : request.Currency;
        entity.UpdatedAtUtc = clock.UtcNow;

        await db.SaveChangesAsync(ct);

        return Result<EventDto>.Success(ToDto(entity));
    }

    public async Task<Result> SetPublishedStatusAsync(int id, bool isPublished, CancellationToken ct = default)
    {
        var entity = await db.Events.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null)
            return Result.Failure($"Event with id {id} not found.", ErrorCodes.NotFound);

        entity.IsPublished = isPublished;
        entity.Status = isPublished ? EventStatus.Published : EventStatus.Draft;
        entity.UpdatedAtUtc = clock.UtcNow;
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.Events.FirstOrDefaultAsync(e => e.Id == id, ct);
        if (entity is null)
            return Result.Failure($"Event with id {id} not found.", ErrorCodes.NotFound);

        db.Events.Remove(entity);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    private static EventDto ToDto(Event e) => new(
        e.Id, e.Title, e.Slug, e.Description, e.LocationName, e.AddressLine,
        e.City, e.Region, e.PostalCode, e.Country, e.StartsAtUtc, e.EndsAtUtc,
        e.BusinessId, e.OrganizerName, e.OrganizerPhoneNumber, e.OrganizerEmail,
        e.CoverImageUrl, e.IsFree, e.Price, e.Currency, e.Status, e.IsPublished,
        e.CreatedByUserId, e.CreatedAtUtc, e.UpdatedAtUtc);

    private static EventListItemDto ToListItemDto(Event e) => new(
        e.Id, e.Title, e.Slug, e.City, e.StartsAtUtc, e.IsFree, e.Price, e.Currency, e.Status, e.IsPublished, e.CoverImageUrl, e.BusinessId);
}
