using Microsoft.EntityFrameworkCore;
using PersianHub.API.Auth;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Layer2Core;
using PersianHub.API.Entities.Layer2Core;
using PersianHub.API.Interfaces.Layer2Core;

namespace PersianHub.API.Services.Layer2Core;

public sealed class BusinessImageService(
    ApplicationDbContext db,
    ICurrentUserService currentUser,
    IDateTimeProvider clock) : IBusinessImageService
{
    public async Task<Result<IReadOnlyList<BusinessImageDto>>> GetByBusinessIdAsync(int businessId, CancellationToken ct = default)
    {
        var images = await db.BusinessImages
            .AsNoTracking()
            .Where(i => i.BusinessId == businessId)
            .OrderBy(i => i.DisplayOrder)
            .Select(i => new BusinessImageDto(i.Id, i.ImageUrl, i.AltText, i.DisplayOrder, i.IsCover))
            .ToListAsync(ct);

        return Result<IReadOnlyList<BusinessImageDto>>.Success(images);
    }

    public async Task<Result<BusinessImageDto>> AddAsync(int businessId, AddBusinessImageDto request, CancellationToken ct = default)
    {
        var business = await db.Businesses.FirstOrDefaultAsync(b => b.Id == businessId, ct);
        if (business is null)
            return Result<BusinessImageDto>.Failure($"Business with id {businessId} not found.", ErrorCodes.NotFound);

        if (!CanManage(business))
            return Result<BusinessImageDto>.Failure("You do not have permission to manage this business.", ErrorCodes.Forbidden);

        var maxOrder = await db.BusinessImages
            .Where(i => i.BusinessId == businessId)
            .MaxAsync(i => (int?)i.DisplayOrder, ct) ?? 0;

        // If this is set as cover, clear existing cover flags
        if (request.IsCover)
        {
            await db.BusinessImages
                .Where(i => i.BusinessId == businessId && i.IsCover)
                .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsCover, false), ct);
        }

        var image = new BusinessImage
        {
            BusinessId = businessId,
            ImageUrl = request.ImageUrl,
            AltText = request.AltText,
            DisplayOrder = maxOrder + 1,
            IsCover = request.IsCover,
            CreatedAtUtc = clock.UtcNow
        };

        db.BusinessImages.Add(image);
        await db.SaveChangesAsync(ct);

        return Result<BusinessImageDto>.Success(
            new BusinessImageDto(image.Id, image.ImageUrl, image.AltText, image.DisplayOrder, image.IsCover));
    }

    public async Task<Result> RemoveAsync(int businessId, int imageId, CancellationToken ct = default)
    {
        var image = await db.BusinessImages
            .Include(i => i.Business)
            .FirstOrDefaultAsync(i => i.Id == imageId && i.BusinessId == businessId, ct);

        if (image is null)
            return Result.Failure("Image not found.", ErrorCodes.NotFound);

        if (!CanManage(image.Business))
            return Result.Failure("You do not have permission to manage this business.", ErrorCodes.Forbidden);

        db.BusinessImages.Remove(image);
        await db.SaveChangesAsync(ct);

        return Result.Success();
    }

    public async Task<Result> SetCoverAsync(int businessId, int imageId, CancellationToken ct = default)
    {
        var business = await db.Businesses.FirstOrDefaultAsync(b => b.Id == businessId, ct);
        if (business is null)
            return Result.Failure($"Business with id {businessId} not found.", ErrorCodes.NotFound);

        if (!CanManage(business))
            return Result.Failure("You do not have permission to manage this business.", ErrorCodes.Forbidden);

        await db.BusinessImages
            .Where(i => i.BusinessId == businessId)
            .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsCover, false), ct);

        var updated = await db.BusinessImages
            .Where(i => i.Id == imageId && i.BusinessId == businessId)
            .ExecuteUpdateAsync(s => s.SetProperty(i => i.IsCover, true), ct);

        if (updated == 0)
            return Result.Failure("Image not found.", ErrorCodes.NotFound);

        return Result.Success();
    }

    private bool CanManage(Business business)
    {
        var role = currentUser.GetRole();
        if (role == AppRoles.Admin) return true;
        var userId = currentUser.GetUserId();
        return userId != 0 && business.OwnerUserId == userId;
    }
}
