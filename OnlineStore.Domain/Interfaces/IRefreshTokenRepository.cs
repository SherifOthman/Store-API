using OnlineStore.Domain.Entities;

namespace OnlineStore.Domain.Interfaces;
public interface IRefreshTokenRepository
{
    public Task<RefreshToken?> GetByValueAsync(string value);

    public Task<int> AddAsync(RefreshToken refreshToken);

    public Task UpdateAsync(RefreshToken refreshToken);

}
