using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer3Network;

namespace PersianHub.API.Interfaces.Layer3Network;

public interface IBusinessSuggestionService
{
    Task<Result<BusinessSuggestionDto>> CreateAsync(CreateBusinessSuggestionDto request, CancellationToken ct = default);
    Task<Result<BusinessSuggestionDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<IReadOnlyList<BusinessSuggestionListItemDto>>> GetByUserIdAsync(int suggestedByUserId, CancellationToken ct = default);
    Task<Result<IReadOnlyList<BusinessSuggestionListItemDto>>> GetAllAsync(CancellationToken ct = default);
    Task<Result<BusinessSuggestionDto>> UpdateStatusAsync(int id, UpdateBusinessSuggestionStatusDto request, CancellationToken ct = default);
}
