using PersianHub.API.Common;
using PersianHub.API.DTOs.Admin;
using PersianHub.API.DTOs.Search;
using PersianHub.API.Enums.Layer2Core;

namespace PersianHub.API.Interfaces.Admin;

/// <summary>
/// Admin payment visibility backed by the Subscription table.
/// Read-only — payments are created by the payment gateway flow, not by admin.
/// </summary>
public interface IAdminPaymentService
{
    Task<PagedResult<AdminPaymentListItemDto>> GetAllAsync(
        int? businessId, PaymentStatus? paymentStatus,
        DateTime? fromUtc, DateTime? toUtc,
        string? search,
        int page, int pageSize, CancellationToken ct);

    Task<Result<AdminPaymentDetailDto>> GetByIdAsync(int subscriptionId, CancellationToken ct);
}
