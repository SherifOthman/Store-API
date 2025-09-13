using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnlineStore.Application.Common;
using OnlineStore.Application.Options;
using OnlineStore.Application.Requests;
using OnlineStore.Application.Responses;
using System.Threading.Tasks;

namespace OnlineStore.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly JwtOptions _options;

    public AuthController(
        IAuthService service,
        IOptions<JwtOptions> options,
        ILogger<AuthController> logger)
    {
        _authService = service;
        _logger = logger;
        _options = options.Value;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
    {
        var result = await _authService.SignUpAsync(request);
        if (!result.Success)
            return BadRequest(result);

        _logger.LogInformation("Signed up user: {email}", request.Email);
        return NoContent();
    }

    [HttpPost("signin")]
    public async Task<ActionResult<Result<AuthResponse>>> SignIn([FromBody] SignInRequest request)
    {
        var authResponse = await _authService.SignInAsync(request);
        if (!authResponse.Success)
            return Unauthorized(authResponse);

        SetRefreshTokenInCookies(authResponse.Data!.RefreshToken);

        authResponse.Data.RefreshToken = string.Empty;

        _logger.LogInformation("User ({email}) signed in", request.Email);

        return Ok(authResponse);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<Result<AuthResponse>>> RefreshToken()
    {
        if (!Request.Cookies.ContainsKey("RefreshToken"))
            return Unauthorized("Invalid RefreshToken");

        var authResponse = await _authService.RefreshTokenAsync(Request.Cookies["RefreshToken"]!);
        if (!authResponse.Success)
            return Unauthorized(authResponse);

        SetRefreshTokenInCookies(authResponse.Data!.RefreshToken);

        authResponse.Data.RefreshToken = string.Empty;

        return Ok(authResponse);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if (Request.Cookies.ContainsKey("RefreshToken"))
        {
            await _authService.Logout(Request.Cookies["RefreshToken"]!);
            Response.Cookies.Delete("RefreshToken");

            _logger.LogInformation("User logged out and refresh token cookie cleared");
        }

        return NoContent();
    }

    private void SetRefreshTokenInCookies(string refreshToken)
    {
        Response.Cookies.Append("RefreshToken", refreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(_options.RefreshTokenExpiry)
                // Secure = true; // Uncomment in production with HTTPS
            });
    }
}
