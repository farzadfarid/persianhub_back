using Microsoft.EntityFrameworkCore;
using PersianHub.API.Auth;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.Enums.Layer2Core;
using PersianHub.API.Interfaces;
using PersianHub.API.Interfaces.Admin;

namespace PersianHub.API.Services.Admin;

public sealed class AdminBusinessClaimService(
    ApplicationDbContext db,
    IDateTimeProvider clock,
    ICurrentUserService currentUser,
    IAuditLogService audit) : IAdminBusinessClaimService
{
    public async Task<Result<IReadOnlyList<BusinessClaimListItemDto>>> GetAllAsync(CancellationToken ct = default)
    {
        var items = await db.BusinessClaimRequests
            .AsNoTracking()
            .Include(c => c.Business)
            .OrderByDescending(c => c.CreatedAtUtc)
            .Select(c => new BusinessClaimListItemDto(
                c.Id, c.BusinessId, c.Business.Name, c.AppUserId, c.Status, c.CreatedAtUtc))
            .ToListAsync(ct);

        return Result<IReadOnlyList<BusinessClaimListItemDto>>.Success(items);
    }

    public async Task<Result<BusinessClaimDetailDto>> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var claim = await db.BusinessClaimRequests
            .AsNoTracking()
            .Include(c => c.Business)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (claim is null)
            return Result<BusinessClaimDetailDto>.Failure($"Business claim request with id {id} not found.", ErrorCodes.NotFound);

        return Result<BusinessClaimDetailDto>.Success(ToDetailDto(claim, claim.Business.Name));
    }

    public async Task<Result<BusinessClaimDetailDto>> ApproveAsync(int id, CancellationToken ct = default)
    {
        var claim = await db.BusinessClaimRequests
            .Include(c => c.Business)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (claim is null)
            return Result<BusinessClaimDetailDto>.Failure($"Business claim request with id {id} not found.", ErrorCodes.NotFound);

        if (claim.Status != BusinessClaimRequestStatus.Pending)
            return Result<BusinessClaimDetailDto>.Failure("Only pending claim requests can be approved.", ErrorCodes.Conflict);

        var claimant = await db.AppUsers.FirstOrDefaultAsync(u => u.Id == claim.AppUserId, ct);
        if (claimant is null)
            return Result<BusinessClaimDetailDto>.Failure($"Claimant user with id {claim.AppUserId} not found.", ErrorCodes.NotFound);

        var now = clock.UtcNow;

        // Assign ownership to the claimant.
        claim.Business.OwnerUserId = claim.AppUserId;
        claim.Business.IsClaimed = true;
        claim.Business.UpdatedAtUtc = now;

        // Promote claimant to BusinessOwner if still User.
        if (claimant.Role == AppRoles.User)
        {
            claimant.Role = AppRoles.BusinessOwner;
            claimant.UpdatedAtUtc = now;
        }

        claim.Status = BusinessClaimRequestStatus.Approved;
        claim.ReviewedByUserId = currentUser.GetUserId();
        claim.ReviewedAtUtc = now;
        claim.UpdatedAtUtc = now;

        await db.SaveChangesAsync(ct);

        await audit.WriteAsync(AuditActions.BusinessClaimApproved, "BusinessClaimRequest", claim.Id.ToString(),
            new { claim.BusinessId, claimantUserId = claim.AppUserId }, ct);

        return Result<BusinessClaimDetailDto>.Success(ToDetailDto(claim, claim.Business.Name));
    }

    public async Task<Result<BusinessClaimDetailDto>> RejectAsync(int id, CancellationToken ct = default)
    {
        var claim = await db.BusinessClaimRequests
            .Include(c => c.Business)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        if (claim is null)
            return Result<BusinessClaimDetailDto>.Failure($"Business claim request with id {id} not found.", ErrorCodes.NotFound);

        if (claim.Status != BusinessClaimRequestStatus.Pending)
            return Result<BusinessClaimDetailDto>.Failure("Only pending claim requests can be rejected.", ErrorCodes.Conflict);

        var now = clock.UtcNow;
        claim.Status = BusinessClaimRequestStatus.Rejected;
        claim.ReviewedByUserId = currentUser.GetUserId();
        claim.ReviewedAtUtc = now;
        claim.UpdatedAtUtc = now;

        await db.SaveChangesAsync(ct);

        await audit.WriteAsync(AuditActions.BusinessClaimRejected, "BusinessClaimRequest", claim.Id.ToString(),
            new { claim.BusinessId, claimantUserId = claim.AppUserId }, ct);

        return Result<BusinessClaimDetailDto>.Success(ToDetailDto(claim, claim.Business.Name));
    }

    private static BusinessClaimDetailDto ToDetailDto(
        PersianHub.API.Entities.Layer2Core.BusinessClaimRequest c, string businessName) => new(
        c.Id, c.BusinessId, businessName, c.AppUserId,
        c.SubmittedBusinessEmail, c.SubmittedPhoneNumber, c.Message,
        c.Status, c.ReviewedByUserId, c.ReviewedAtUtc,
        c.CreatedAtUtc, c.UpdatedAtUtc);
}
