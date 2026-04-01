using Microsoft.EntityFrameworkCore;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Entities.Layer3Network;
using PersianHub.API.Interfaces.Layer3Network;

namespace PersianHub.API.Services.Layer3Network;

public sealed class BusinessSuggestionService(ApplicationDbContext db, IDateTimeProvider clock) : IBusinessSuggestionService
{
    public async Task<Result<BusinessSuggestionDto>> CreateAsync(CreateBusinessSuggestionDto request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.BusinessName))
            return Result<BusinessSuggestionDto>.Failure("Business name is required.", ErrorCodes.ValidationFailed);

        if (request.SuggestedByUserId.HasValue)
        {
            var userExists = await db.AppUsers.AnyAsync(u => u.Id == request.SuggestedByUserId.Value, ct);
            if (!userExists)
                return Result<BusinessSuggestionDto>.Failure($"User with id {request.SuggestedByUserId.Value} not found.", ErrorCodes.NotFound);
        }

        var now = clock.UtcNow;
        var entity = new BusinessSuggestion
        {
            SuggestedByUserId = request.SuggestedByUserId,
            BusinessName = request.BusinessName.Trim(),
            CategoryText = request.CategoryText?.Trim(),
            PhoneNumber = request.PhoneNumber?.Trim(),
            Email = request.Email?.Trim(),
            Website = request.Website?.Trim(),
            AddressLine = request.AddressLine?.Trim(),
            City = request.City?.Trim(),
            Description = request.Description?.Trim(),
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        db.BusinessSuggestions.Add(entity);
        await db.SaveChangesAsync(ct);

        return Result<BusinessSuggestionDto>.Success(ToDto(entity));
    }

    public async Task<Result<BusinessSuggestionDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var entity = await db.BusinessSuggestions.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id, ct);
        if (entity is null)
            return Result<BusinessSuggestionDto>.Failure($"BusinessSuggestion with id {id} not found.", ErrorCodes.NotFound);

        return Result<BusinessSuggestionDto>.Success(ToDto(entity));
    }

    public async Task<Result<IReadOnlyList<BusinessSuggestionListItemDto>>> GetByUserIdAsync(int suggestedByUserId, CancellationToken ct = default)
    {
        var userExists = await db.AppUsers.AnyAsync(u => u.Id == suggestedByUserId, ct);
        if (!userExists)
            return Result<IReadOnlyList<BusinessSuggestionListItemDto>>.Failure($"User with id {suggestedByUserId} not found.", ErrorCodes.NotFound);

        var items = await db.BusinessSuggestions
            .AsNoTracking()
            .Where(s => s.SuggestedByUserId == suggestedByUserId)
            .OrderByDescending(s => s.CreatedAtUtc)
            .Select(s => new BusinessSuggestionListItemDto(s.Id, s.SuggestedByUserId, s.BusinessName, s.Status, s.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<BusinessSuggestionListItemDto>>.Success(items);
    }

    public async Task<Result<IReadOnlyList<BusinessSuggestionListItemDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await db.BusinessSuggestions
            .AsNoTracking()
            .OrderByDescending(s => s.CreatedAtUtc)
            .Select(s => new BusinessSuggestionListItemDto(s.Id, s.SuggestedByUserId, s.BusinessName, s.Status, s.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<BusinessSuggestionListItemDto>>.Success(items);
    }

    public async Task<Result<BusinessSuggestionDto>> UpdateStatusAsync(int id, UpdateBusinessSuggestionStatusDto request, CancellationToken ct = default)
    {
        var entity = await db.BusinessSuggestions.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (entity is null)
            return Result<BusinessSuggestionDto>.Failure($"BusinessSuggestion with id {id} not found.", ErrorCodes.NotFound);

        if (request.ReviewedByUserId.HasValue)
        {
            var reviewerExists = await db.AppUsers.AnyAsync(u => u.Id == request.ReviewedByUserId.Value, ct);
            if (!reviewerExists)
                return Result<BusinessSuggestionDto>.Failure($"Reviewer user with id {request.ReviewedByUserId.Value} not found.", ErrorCodes.NotFound);
        }

        var now = clock.UtcNow;
        entity.Status = request.Status;
        entity.ReviewedByUserId = request.ReviewedByUserId;
        entity.ReviewedAtUtc = now;
        entity.UpdatedAtUtc = now;

        await db.SaveChangesAsync(ct);

        return Result<BusinessSuggestionDto>.Success(ToDto(entity));
    }

    private static BusinessSuggestionDto ToDto(BusinessSuggestion s) => new(
        s.Id, s.SuggestedByUserId, s.BusinessName, s.CategoryText, s.PhoneNumber,
        s.Email, s.Website, s.AddressLine, s.City, s.Description,
        s.Status, s.ReviewedByUserId, s.ReviewedAtUtc, s.CreatedAtUtc, s.UpdatedAtUtc);
}
