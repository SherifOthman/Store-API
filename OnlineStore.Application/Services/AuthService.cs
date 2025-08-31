using FluentValidation;
using Mapster;
using Microsoft.Extensions.Options;
using OnlineStore.Application.Common;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Options;
using OnlineStore.Application.Providers;
using OnlineStore.Application.Requests;
using OnlineStore.Application.Responses;
using OnlineStore.Application.Services;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Interfaces;

public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IJwtProvider _jwtProvider;
    private readonly JwtOptions _options;

    public AuthService(IUserService userService,
        IRefreshTokenService refreshTokenService,
        IUnitOfWork unitOfWork,
        IJwtProvider jwtProvider,
        IOptions<JwtOptions> options)
    {
        _userService = userService;
        _refreshTokenService = refreshTokenService;
        _jwtProvider = jwtProvider;
        _options = options.Value;
    }

    public async Task<Result> SignUpAsync(SignUpRequest request)
    {
        var authResponse = await _userService.CreateAsync(request);
        if (!authResponse.Success)
            return authResponse;

        return Result.Ok();
    }

    public async Task<Result<AuthResponse>> SignInAsync(SignInRequest request)
    {
        var user = await _userService.GetByEmailAsync(request.Email);
        if (user == null || !_userService.VerifyPassword(user, request.Password))
        {
            return Result<AuthResponse>.Fail("Invalid email or password");
        }

        string accessToken = _jwtProvider.GenerateAccessToken(user, _options);
        string refreshToken = (await _refreshTokenService.GenerateForUserAsync(user.Id,
            _options.RefreshTokenExpiry)).Value;

        return await PrpareAuthResponse(user);

    }

    public async Task<Result<AuthResponse>> RefreshTokenAsync(string refreshToken)
    {
        var existing = await _refreshTokenService.GetByValueAsync(refreshToken);

        if (existing == null || !_refreshTokenService.Validate(existing))
            return Result<AuthResponse>.Fail("Invalid refresh token");

        await _refreshTokenService.RevokeAsync(refreshToken);

        var user = await _userService.GetByIdAsync(existing.UserId);

        return await PrpareAuthResponse(user!);
    }

    private async Task<AuthResponse> PrpareAuthResponse(User user)
    {
        string accessToken = _jwtProvider.GenerateAccessToken(user, _options);
        string refreshToken = (await _refreshTokenService.GenerateForUserAsync(user.Id,
            _options.RefreshTokenExpiry)).Value;

        var AuthResponse = new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = user.Adapt<UserDto>()
        };

        return AuthResponse;
    }
}
