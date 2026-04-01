using PersianHub.API.Auth.DTOs;
using PersianHub.API.Common;

namespace PersianHub.API.Auth;

public interface IAuthService
{
    Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto);
    Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto);
}
