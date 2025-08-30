using OnlineStore.Application.Common;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Interfaces;
using System.Security.Cryptography;

namespace OnlineStore.Application.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IUnitOfWork _unitOfWork;

    public RefreshTokenService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RefreshToken> GenerateForUserAsync(int userId, int days)
    {
        var tokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        var refreshToken = new RefreshToken
        {
            UserId = userId,
            Value = tokenValue,
            IsRevoked = false,
            ExpiryDate = DateTime.UtcNow.AddDays(days)
        };

        await _unitOfWork.RefreshTokens.AddAsync(refreshToken);

        return refreshToken;
    }

    public async Task<RefreshToken?> GetByValueAsync(string tokenValue)
    {
        return await _unitOfWork.RefreshTokens.GetByValueAsync(tokenValue);
    }

    public bool Validate(RefreshToken token)
    {
        if (token == null) return false;


        return !token.IsRevoked && token.ExpiryDate > DateTime.UtcNow;
    }

    public async Task RevokeAsync(string tokenValue)
    {
        var token = await GetByValueAsync(tokenValue);
        if (token == null) return;

        token.IsRevoked = true;
        await _unitOfWork.RefreshTokens.UpdateAsync(token);
    }


}
