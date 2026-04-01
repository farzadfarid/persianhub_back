using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PersianHub.API.Auth.DTOs;
using PersianHub.API.Common;
using PersianHub.API.Data;
using PersianHub.API.Entities.Common;
using PersianHub.API.Interfaces;

namespace PersianHub.API.Auth;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _db;
    private readonly PasswordHasher<AppUser> _passwordHasher;
    private readonly IConfiguration _config;
    private readonly IDateTimeProvider _dateTime;
    private readonly IAuditLogService _audit;

    public AuthService(
        ApplicationDbContext db,
        PasswordHasher<AppUser> passwordHasher,
        IConfiguration config,
        IDateTimeProvider dateTime,
        IAuditLogService audit)
    {
        _db = db;
        _passwordHasher = passwordHasher;
        _config = config;
        _dateTime = dateTime;
        _audit = audit;
    }

    public async Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto)
    {
        var exists = await _db.AppUsers.AnyAsync(u => u.Email == dto.Email);
        if (exists)
        {
            await _audit.WriteAsync(AuditActions.UserRegistrationFailed, "AppUser", null,
                new { reason = "EmailAlreadyExists" });
            return Result<AuthResponseDto>.Failure("Email is already registered.", ErrorCodes.AlreadyExists);
        }

        var role = dto.Role is { Length: > 0 } r &&
                   (r == AppRoles.Admin || r == AppRoles.BusinessOwner || r == AppRoles.User)
            ? r
            : AppRoles.User;

        var user = new AppUser
        {
            Email = dto.Email,
            Role = role,
            IsActive = true,
            CreatedAtUtc = _dateTime.UtcNow,
            UpdatedAtUtc = _dateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

        _db.AppUsers.Add(user);
        await _db.SaveChangesAsync();

        await _audit.WriteAsync(AuditActions.UserRegistered, "AppUser", user.Id.ToString(),
            new { role = user.Role });

        return Result<AuthResponseDto>.Success(BuildResponse(user));
    }

    public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto)
    {
        var user = await _db.AppUsers.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user is null)
        {
            await _audit.WriteAsync(AuditActions.UserLoginFailed, "AppUser", null,
                new { reason = "UserNotFound" });
            return Result<AuthResponseDto>.Failure("Invalid email or password.", ErrorCodes.ValidationFailed);
        }

        PasswordVerificationResult verificationResult;
        try
        {
            verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        }
        catch
        {
            verificationResult = PasswordVerificationResult.Failed;
        }

        if (verificationResult == PasswordVerificationResult.Failed)
        {
            await _audit.WriteAsync(AuditActions.UserLoginFailed, "AppUser", user.Id.ToString(),
                new { reason = "InvalidPassword" });
            return Result<AuthResponseDto>.Failure("Invalid email or password.", ErrorCodes.ValidationFailed);
        }

        await _audit.WriteAsync(AuditActions.UserLoginSucceeded, "AppUser", user.Id.ToString());

        return Result<AuthResponseDto>.Success(BuildResponse(user));
    }

    private AuthResponseDto BuildResponse(AppUser user)
    {
        var expireMinutes = _config.GetValue<int>("Jwt:ExpireMinutes", 60);
        var expiration = _dateTime.UtcNow.AddMinutes(expireMinutes);
        var token = GenerateToken(user, expiration);

        return new AuthResponseDto
        {
            Token = token,
            Expiration = expiration,
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role
        };
    }

    private string GenerateToken(AppUser user, DateTime expiration)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
