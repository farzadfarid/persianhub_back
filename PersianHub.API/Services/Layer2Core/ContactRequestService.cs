using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Entities.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Services.Layer2Core;

public sealed class ContactRequestService(ApplicationDbContext db, IDateTimeProvider clock) : IContactRequestService
{
    public async Task<Result<IReadOnlyList<ContactRequestListItemDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await db.ContactRequests
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAtUtc)
            .Select(c => new ContactRequestListItemDto(
                c.Id, c.BusinessId, c.AppUserId, c.Name, c.ContactType, c.Status, c.IsConverted, c.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<ContactRequestListItemDto>>.Success(items);
    }

    public async Task<Result<ContactRequestDto>> CreateAsync(CreateContactRequestDto request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<ContactRequestDto>.Failure("Name is required.", ErrorCodes.ValidationFailed);

        if (string.IsNullOrWhiteSpace(request.Email))
            return Result<ContactRequestDto>.Failure("Email is required.", ErrorCodes.ValidationFailed);

        var businessExists = await db.Businesses.AnyAsync(b => b.Id == request.BusinessId, ct);
        if (!businessExists)
            return Result<ContactRequestDto>.Failure($"Business with id {request.BusinessId} not found.", ErrorCodes.NotFound);

        if (request.AppUserId.HasValue)
        {
            var userExists = await db.AppUsers.AnyAsync(u => u.Id == request.AppUserId.Value, ct);
            if (!userExists)
                return Result<ContactRequestDto>.Failure($"User with id {request.AppUserId.Value} not found.", ErrorCodes.NotFound);
        }

        var now = clock.UtcNow;
        var entity = new ContactRequest
        {
            BusinessId = request.BusinessId,
            AppUserId = request.AppUserId,
            Name = request.Name.Trim(),
            Email = request.Email.Trim(),
            PhoneNumber = request.PhoneNumber?.Trim(),
            Message = request.Message?.Trim(),
            ContactType = request.ContactType,
            IsConverted = false,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        db.ContactRequests.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<ContactRequestDto>.Success(ToDto(entity));
    }

    public async Task<Result<ContactRequestDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.ContactRequests.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id, ct);
        if (entity is null)
            return Result<ContactRequestDto>.Failure($"ContactRequest with id {id} not found.", ErrorCodes.NotFound);

        return Result<ContactRequestDto>.Success(ToDto(entity));
    }

    public async Task<Result<IReadOnlyList<ContactRequestListItemDto>>> GetByBusinessIdAsync(int businessId, CancellationToken ct = default)
    {
        var businessExists = await db.Businesses.AnyAsync(b => b.Id == businessId, ct);
        if (!businessExists)
            return Result<IReadOnlyList<ContactRequestListItemDto>>.Failure($"Business with id {businessId} not found.", ErrorCodes.NotFound);

        var items = await db.ContactRequests
            .AsNoTracking()
            .Where(c => c.BusinessId == businessId)
            .OrderByDescending(c => c.CreatedAtUtc)
            .Select(c => new ContactRequestListItemDto(
                c.Id, c.BusinessId, c.AppUserId, c.Name, c.ContactType, c.Status, c.IsConverted, c.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<ContactRequestListItemDto>>.Success(items);
    }

    public async Task<Result<IReadOnlyList<ContactRequestListItemDto>>> GetByUserIdAsync(int appUserId, CancellationToken ct = default)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == appUserId, ct);
        if (!userExists)
            return Result<IReadOnlyList<ContactRequestListItemDto>>.Failure($"User with id {appUserId} not found.", ErrorCodes.NotFound);

        var items = await db.ContactRequests
            .AsNoTracking()
            .Where(c => c.AppUserId == appUserId)
            .OrderByDescending(c => c.CreatedAtUtc)
            .Select(c => new ContactRequestListItemDto(
                c.Id, c.BusinessId, c.AppUserId, c.Name, c.ContactType, c.Status, c.IsConverted, c.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<ContactRequestListItemDto>>.Success(items);
    }

    public async Task<Result> MarkConvertedAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.ContactRequests.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (entity is null)
            return Result.Failure($"ContactRequest with id {id} not found.", ErrorCodes.NotFound);

        entity.IsConverted = true;
        entity.UpdatedAtUtc = clock.UtcNow;
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    private static ContactRequestDto ToDto(ContactRequest c) => new(
        c.Id, c.BusinessId, c.AppUserId, c.Name, c.Email, c.PhoneNumber,
        c.Message, c.ContactType, c.Status, c.IsConverted, c.Metadata,
        c.CreatedAtUtc, c.UpdatedAtUtc);
}
