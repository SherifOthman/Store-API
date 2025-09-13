using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Services;
public interface IRefreshTokenService
{
    Task<RefreshToken> GenerateForUserAsync(int userId, int days);
    Task<RefreshToken?> GetByValueAsync(string tokenValue);
    Task RevokeAsync(string tokenValue);
    bool Validate(RefreshToken token);


}