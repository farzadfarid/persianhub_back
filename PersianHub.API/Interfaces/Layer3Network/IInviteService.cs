using PersianHub.API.Common;
using PersianHub.API.DTOs.Layer3Network;

namespace PersianHub.API.Interfaces.Layer3Network;

public interface IInviteService
{
    Task<Result<InviteDto>> CreateAsync(CreateInviteDto request, CancellationToken ct = default);
    Task<Result<InviteDto>> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Result<IReadOnlyList<InviteListItemDto>>> GetBySenderUserIdAsync(int inviterUserId, CancellationToken ct = default);
    Task<Result<InviteDto>> UpdateStatusAsync(int id, UpdateInviteStatusDto request, CancellationToken ct = default);
}
