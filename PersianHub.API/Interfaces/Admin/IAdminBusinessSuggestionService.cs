using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer3Network;

namespace PersianHub.API.Interfaces.Admin;

public interface IAdminBusinessSuggestionService
{
    Task<Result<IReadOnlyList<BusinessSuggestionListItemDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<BusinessSuggestionDto>> GetByIdAsync(int id, CancellationToken ct = default);
    /// <summary>
    /// Approves the suggestion and creates a Business record from it.
    /// The created business has no owner (OwnerUserId = null, IsClaimed = false)
    /// because suggestions do not carry a verified owner identity.
    /// </summary>
    Task<Result<BusinessSuggestionDto>> ApproveAsync(int id, CancellationToken ct = default);
    Task<Result<BusinessSuggestionDto>> RejectAsync(int id, CancellationToken ct = default);
}
