namespace PersianHub.API.Auth.DTOs;

public class RegisterDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    /// <summary>Optional. Defaults to "User" if not provided.</summary>
    public string? Role { get; set; }
}
