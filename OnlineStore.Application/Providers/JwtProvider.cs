using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using OnlineStore.Application.Options;
using OnlineStore.Domain.Entities;
using OnlineStore.Domain.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineStore.Application.Providers;
internal class JwtProvider : IJwtProvider
{

    public string GenerateAccessToken(User user, JwtOptions options)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecurityKey)),
            SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Issuer = options.Issuer,
            Audience = options.Audience,
            Subject = new ClaimsIdentity(_GetUserClaims(user)),
            Expires = DateTime.UtcNow.AddHours(options.AccessTokenExpiry),
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(securityToken);

        return accessToken;
    }

    private List<Claim> _GetUserClaims(User user)
    {
        var userClaims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        };

        _GetUserRoles(user).ForEach(role =>
            userClaims.Add(new Claim(ClaimTypes.Role, role)));


        return userClaims;

    }

    private List<string> _GetUserRoles(User user)
    {
        // System roles as enums
        RoleValue[] roles = Enum.GetValues<RoleValue>();
        List<string> userRolesAsString = new List<string>();


        foreach (var role in roles)
        {
            // Match each enum role with user roles
            if (user.Role.HasFlag(role))
            {
                userRolesAsString.Add(role.ToString());
            }
        }

        return userRolesAsString;

    }
}
