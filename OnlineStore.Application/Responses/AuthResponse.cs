using OnlineStore.Application.DTOs;

namespace OnlineStore.Application.Responses;

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public UserDto User { get; set; } = default!;
}