
namespace OnlineStore.Application.Options;
public class JwtOptions
{
     public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string SecurityKey { get; set; } = default!;
    public int AccessTokenExpiry { get; set; }
    public int RefreshTokenExpiry { get; set; }
}
