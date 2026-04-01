using Microsoft.EntityFrameworkCore;
using PersianHub.API.Auth;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer3Network;
using PersianHub.API.Entities.Layer2Core;
using PersianHub.API.Enums.Layer2Core;
using PersianHub.API.Interfaces;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

public sealed class AdminBusinessSuggestionService(
    ApplicationDbContext db,
    IDateTimeProvider clock,
    ICurrentUserService currentUser,
    IAuditLogService audit) : IAdminBusinessSuggestionService
{
    public async Task<Result<IReadOnlyList<BusinessSuggestionListItemDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await db.BusinessSuggestions
            .AsNoTracking()
            .OrderByDescending(s => s.CreatedAtUtc)
            .Select(s => new BusinessSuggestionListItemDto(s.Id, s.SuggestedByUserId, s.BusinessName, s.Status, s.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<BusinessSuggestionListItemDto>>.Success(items);
    }

    public async Task<Result<BusinessSuggestionDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var s = await db.BusinessSuggestions.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        if (s is null)
            return Result<BusinessSuggestionDto>.Failure($"Business suggestion with id {id} not found.", ErrorCodes.NotFound);

        return Result<BusinessSuggestionDto>.Success(ToDto(s));
    }

    /// <summary>
    /// Approves the suggestion, marks it Approved, and creates a Business record.
    /// Business is created without an owner (OwnerUserId = null, IsClaimed = false) because
    /// a community suggestion does not carry verified ownership — the platform operator
    /// may assign an owner separately via a claim request or admin action.
    /// </summary>
    public async Task<Result<BusinessSuggestionDto>> ApproveAsync(int id, CancellationToken ct = default)
    {
        var suggestion = await db.BusinessSuggestions.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (suggestion is null)
            return Result<BusinessSuggestionDto>.Failure($"Business suggestion with id {id} not found.", ErrorCodes.NotFound);

        if (suggestion.Status != BusinessClaimRequestStatus.Pending)
            return Result<BusinessSuggestionDto>.Failure("Only pending suggestions can be approved.", ErrorCodes.Conflict);

        var now = clock.UtcNow;

        // Derive a unique slug from the suggested business name.
        var baseSlug = SlugHelper.Generate(suggestion.BusinessName);
        var existingSlugs = await db.Businesses
            .Where(b => b.Slug.StartsWith(baseSlug))
            .Select(b => b.Slug)
            .ToListAsync(ct);
        var slug = SlugHelper.MakeUnique(baseSlug, existingSlugs);

        var business = new Business
        {
            Name = suggestion.BusinessName.Trim(),
            Slug = slug,
            Description = suggestion.Description?.Trim(),
            PhoneNumber = suggestion.PhoneNumber?.Trim(),
            Email = suggestion.Email?.Trim(),
            Website = suggestion.Website?.Trim(),
            AddressLine = suggestion.AddressLine?.Trim(),
            City = suggestion.City?.Trim(),
            Country = "Sweden",
            OwnerUserId = null,     // No verified owner — assign via claim flow later
            IsClaimed = false,
            IsActive = true,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        db.Businesses.Add(business);

        suggestion.Status = BusinessClaimRequestStatus.Approved;
        suggestion.ReviewedByUserId = currentUser.GetUserId();
        suggestion.ReviewedAtUtc = now;
        suggestion.UpdatedAtUtc = now;

        await db.SaveChangesAsync(ct);

        await audit.WriteAsync(AuditActions.BusinessSuggestionApproved, "BusinessSuggestion", suggestion.Id.ToString(),
            new { business.Name, business.Id }, ct);

        return Result<BusinessSuggestionDto>.Success(ToDto(suggestion));
    }

    public async Task<Result<BusinessSuggestionDto>> RejectAsync(int id, CancellationToken ct = default)
    {
        var suggestion = await db.BusinessSuggestions.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (suggestion is null)
            return Result<BusinessSuggestionDto>.Failure($"Business suggestion with id {id} not found.", ErrorCodes.NotFound);

        if (suggestion.Status != BusinessClaimRequestStatus.Pending)
            return Result<BusinessSuggestionDto>.Failure("Only pending suggestions can be rejected.", ErrorCodes.Conflict);

        var now = clock.UtcNow;
        suggestion.Status = BusinessClaimRequestStatus.Rejected;
        suggestion.ReviewedByUserId = currentUser.GetUserId();
        suggestion.ReviewedAtUtc = now;
        suggestion.UpdatedAtUtc = now;

        await db.SaveChangesAsync(ct);

        await audit.WriteAsync(AuditActions.BusinessSuggestionRejected, "BusinessSuggestion", suggestion.Id.ToString(),
            new { suggestion.BusinessName }, ct);

        return Result<BusinessSuggestionDto>.Success(ToDto(suggestion));
    }

    private static BusinessSuggestionDto ToDto(PersianHub.API.Entities.Layer3Network.BusinessSuggestion s) => new(
        s.Id, s.SuggestedByUserId, s.BusinessName, s.CategoryText, s.PhoneNumber,
        s.Email, s.Website, s.AddressLine, s.City, s.Description,
        s.Status, s.ReviewedByUserId, s.ReviewedAtUtc, s.CreatedAtUtc, s.UpdatedAtUtc);
}
