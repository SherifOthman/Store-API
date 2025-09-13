using OnlineStore.Application.Common;
using OnlineStore.Application.Requests;
using OnlineStore.Application.Responses;

public interface IAuthService
{
    Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken);
    Task<Result<AuthResponse>> SignInAsync(SignInRequest request);
    Task<Result> SignUpAsync(SignUpRequest request);

    Task Logout(string refreshToken);
}