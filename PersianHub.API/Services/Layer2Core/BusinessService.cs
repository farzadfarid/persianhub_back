using Microsoft.EntityFrameworkCore;
using PersianHub.API.Auth;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Entities.Layer2Core;
using PersianHub.API.Interfaces;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Services.Layer2Core;

public sealed class BusinessService(
    ApplicationDbContext db,
    IDateTimeProvider clock,
    ICurrentUserService currentUser,
    IAuditLogService audit) : IBusinessService
{
    public async Task<Result<BusinessDetailsDto>> CreateAsync(CreateBusinessRequestDto request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<BusinessDetailsDto>.Failure("Business name is required.", ErrorCodes.ValidationFailed);

        var userId = currentUser.GetUserId();
        if (userId == 0)
            return Result<BusinessDetailsDto>.Failure("Authentication required to create a business.", ErrorCodes.Forbidden);

        // Resolve slug — use provided value or generate from name, then ensure uniqueness.
        var baseSlug = string.IsNullOrWhiteSpace(request.Slug)
            ? SlugHelper.Generate(request.Name)
            : SlugHelper.Generate(request.Slug);

        if (string.IsNullOrEmpty(baseSlug))
            return Result<BusinessDetailsDto>.Failure("Could not generate a valid slug from the provided name.", ErrorCodes.ValidationFailed);

        var existingSlugs = await db.Businesses
            .Where(b => b.Slug.StartsWith(baseSlug))
            .Select(b => b.Slug)
            .ToListAsync(ct);

        var slug = SlugHelper.MakeUnique(baseSlug, existingSlugs);

        var now = clock.UtcNow;
        var business = new Business
        {
            Name = request.Name.Trim(),
            NameFa = request.NameFa?.Trim(),
            Slug = slug,
            Description = request.Description?.Trim(),
            DescriptionFa = request.DescriptionFa?.Trim(),
            PhoneNumber = request.PhoneNumber?.Trim(),
            Email = request.Email?.Trim(),
            Website = request.Website?.Trim(),
            InstagramUrl = request.InstagramUrl?.Trim(),
            TelegramUrl = request.TelegramUrl?.Trim(),
            WhatsAppNumber = request.WhatsAppNumber?.Trim(),
            AddressLine = request.AddressLine?.Trim(),
            AddressLineFa = request.AddressLineFa?.Trim(),
            City = request.City?.Trim(),
            CityFa = request.CityFa?.Trim(),
            Region = request.Region?.Trim(),
            RegionFa = request.RegionFa?.Trim(),
            PostalCode = request.PostalCode?.Trim(),
            Country = string.IsNullOrWhiteSpace(request.Country) ? "Sweden" : request.Country.Trim(),
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            LogoUrl = request.LogoUrl?.Trim(),
            OwnerUserId = userId,
            IsClaimed = true,   // Direct creation by authenticated user = claimed at birth
            IsActive = true,
            CreatedAtUtc = now,
            UpdatedAtUtc = now
        };

        db.Businesses.Add(business);

        // Promote User → BusinessOwner if their role is still the base User role.
        var userRole = currentUser.GetRole();
        if (userRole == AppRoles.User)
        {
            var appUser = await db.AppUsers.FirstOrDefaultAsync(u => u.Id == userId, ct);
            if (appUser is not null)
            {
                appUser.Role = AppRoles.BusinessOwner;
                appUser.UpdatedAtUtc = now;
            }
        }

        await db.SaveChangesAsync(ct);

        await audit.WriteAsync(AuditActions.BusinessCreated, "Business", business.Id.ToString(),
            new { business.Name, business.Slug, business.City, ownerId = userId });

        return Result<BusinessDetailsDto>.Success(ToDetailsDto(business));
    }

    public async Task<Result<BusinessDetailsDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var business = await db.Businesses
            .AsNoTracking()
            .Include(b => b.Images.OrderBy(i => i.DisplayOrder))
            .FirstOrDefaultAsync(b => b.Id == id, ct);
        if (business is null)
            return Result<BusinessDetailsDto>.Failure($"Business with id {id} not found.", ErrorCodes.NotFound);

        return Result<BusinessDetailsDto>.Success(ToDetailsDto(business));
    }

    public async Task<Result<BusinessDetailsDto>> GetBySlugAsync(string slug, CancellationToken ct = default)
    {
        var business = await db.Businesses
            .AsNoTracking()
            .Include(b => b.Images.OrderBy(i => i.DisplayOrder))
            .FirstOrDefaultAsync(b => b.Slug == slug, ct);

        if (business is null)
            return Result<BusinessDetailsDto>.Failure($"Business with slug '{slug}' not found.", ErrorCodes.NotFound);

        return Result<BusinessDetailsDto>.Success(ToDetailsDto(business));
    }

    public async Task<Result<IReadOnlyList<BusinessListItemDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var businesses = await db.Businesses
            .AsNoTracking()
            .OrderByDescending(b => b.IsFeatured)
            .ThenBy(b => b.Name)
            .Select(b => new BusinessListItemDto(
                b.Id, b.Name, b.NameFa, b.Slug, b.City, b.CityFa, b.PhoneNumber, b.LogoUrl, b.IsVerified, b.IsFeatured, b.IsActive))
            .ToListAsync(ct);

        return Result<IReadOnlyList<BusinessListItemDto>>.Success(businesses);
    }

    public async Task<Result<IReadOnlyList<BusinessListItemDto>>> GetMyBusinessesAsync(CancellationToken ct = default)
    {
        var userId = currentUser.GetUserId();
        if (userId == 0)
            return Result<IReadOnlyList<BusinessListItemDto>>.Failure("Authentication required.", ErrorCodes.Forbidden);

        var businesses = await db.Businesses
            .AsNoTracking()
            .Where(b => b.OwnerUserId == userId)
            .OrderByDescending(b => b.IsFeatured)
            .ThenBy(b => b.Name)
            .Select(b => new BusinessListItemDto(
                b.Id, b.Name, b.NameFa, b.Slug, b.City, b.CityFa, b.PhoneNumber, b.LogoUrl, b.IsVerified, b.IsFeatured, b.IsActive))
            .ToListAsync(ct);

        return Result<IReadOnlyList<BusinessListItemDto>>.Success(businesses);
    }

    public async Task<Result<BusinessDetailsDto>> UpdateAsync(int id, UpdateBusinessRequestDto request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<BusinessDetailsDto>.Failure("Business name is required.", ErrorCodes.ValidationFailed);

        var business = await db.Businesses.FirstOrDefaultAsync(b => b.Id == id, ct);
        if (business is null)
            return Result<BusinessDetailsDto>.Failure($"Business with id {id} not found.", ErrorCodes.NotFound);

        var ownershipCheck = CheckOwnership(business);
        if (!ownershipCheck.IsSuccess)
            return Result<BusinessDetailsDto>.Failure(ownershipCheck.Error!, ownershipCheck.ErrorCode);

        business.Name = request.Name.Trim();
        business.NameFa = request.NameFa?.Trim();
        business.Description = request.Description?.Trim();
        business.DescriptionFa = request.DescriptionFa?.Trim();
        business.PhoneNumber = request.PhoneNumber?.Trim();
        business.Email = request.Email?.Trim();
        business.Website = request.Website?.Trim();
        business.InstagramUrl = request.InstagramUrl?.Trim();
        business.TelegramUrl = request.TelegramUrl?.Trim();
        business.WhatsAppNumber = request.WhatsAppNumber?.Trim();
        business.AddressLine = request.AddressLine?.Trim();
        business.AddressLineFa = request.AddressLineFa?.Trim();
        business.City = request.City?.Trim();
        business.CityFa = request.CityFa?.Trim();
        business.Region = request.Region?.Trim();
        business.RegionFa = request.RegionFa?.Trim();
        business.PostalCode = request.PostalCode?.Trim();
        business.Country = string.IsNullOrWhiteSpace(request.Country) ? "Sweden" : request.Country.Trim();
        business.Latitude = request.Latitude;
        business.Longitude = request.Longitude;
        business.LogoUrl = request.LogoUrl?.Trim();
        business.UpdatedAtUtc = clock.UtcNow;

        await db.SaveChangesAsync(ct);

        await audit.WriteAsync(AuditActions.BusinessUpdated, "Business", business.Id.ToString(),
            new { business.Name, business.City });

        return Result<BusinessDetailsDto>.Success(ToDetailsDto(business));
    }

    public async Task<Result> SetActiveStatusAsync(int id, bool isActive, CancellationToken ct = default)
    {
        var business = await db.Businesses.FirstOrDefaultAsync(b => b.Id == id, ct);
        if (business is null)
            return Result.Failure($"Business with id {id} not found.", ErrorCodes.NotFound);

        var ownershipCheck = CheckOwnership(business);
        if (!ownershipCheck.IsSuccess)
            return ownershipCheck;

        business.IsActive = isActive;
        business.UpdatedAtUtc = clock.UtcNow;
        await db.SaveChangesAsync(ct);

        var action = isActive ? AuditActions.BusinessActivated : AuditActions.BusinessDeactivated;
        await audit.WriteAsync(action, "Business", business.Id.ToString(), new { business.Name });

        return Result.Success();
    }

    /// <summary>
    /// Returns Success if the current user is Admin or owns the given business.
    /// Returns a Forbidden failure otherwise.
    /// </summary>
    private Result CheckOwnership(Business business)
    {
        var role = currentUser.GetRole();
        if (role == AppRoles.Admin)
            return Result.Success();

        var userId = currentUser.GetUserId();
        if (userId != 0 && business.OwnerUserId == userId)
            return Result.Success();

        return Result.Failure("You do not have permission to manage this business.", ErrorCodes.Forbidden);
    }

    private static BusinessDetailsDto ToDetailsDto(Business b) => new(
        b.Id, b.Name, b.NameFa, b.Slug, b.LogoUrl, b.Description, b.DescriptionFa,
        b.PhoneNumber, b.Email, b.Website, b.InstagramUrl, b.TelegramUrl, b.WhatsAppNumber,
        b.AddressLine, b.AddressLineFa, b.City, b.CityFa, b.Region, b.RegionFa,
        b.PostalCode, b.Country, b.Latitude, b.Longitude,
        b.IsVerified, b.IsClaimed, b.IsFeatured, b.IsActive, b.OwnerUserId,
        b.CreatedAtUtc, b.UpdatedAtUtc,
        b.Images.OrderBy(i => i.DisplayOrder)
                .Select(i => new BusinessImageDto(i.Id, i.ImageUrl, i.AltText, i.DisplayOrder, i.IsCover))
                .ToList());
}
