using OnlineStore.Application.Options;
using OnlineStore.Domain.Entities;

namespace OnlineStore.Application.Providers;
public interface IJwtProvider
{
    string GenerateAccessToken(User user, JwtOptions options);
}