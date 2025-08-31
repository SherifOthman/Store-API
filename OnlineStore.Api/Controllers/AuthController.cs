using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OnlineStore.Api.utils;
using OnlineStore.Application.Common;
using OnlineStore.Application.Options;
using OnlineStore.Application.Requests;
using OnlineStore.Application.Responses;

namespace OnlineStore.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;
    private readonly ILogger<AuthController> _logger;
    private readonly JwtOptions _options;

    public AuthController(IAuthService service,
        ILoggedInUser loggedInUser,
        IOptions<JwtOptions> options,
        ILogger<AuthController> logger)
    {
        _service = service;
        _logger = logger;
        _options = options.Value;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
    {
        var result = await _service.SignUpAsync(request);
        if (!result.Success)
            return BadRequest(result);

        _logger.LogInformation("Signed up user: {email}", request.Email);

        return NoContent();
    }

    [HttpPost("signin")]
    public async Task<ActionResult<Result<AuthResponse>>> SignIn([FromBody] SignInRequestWithFlag request)
    {
        var authResponse = await _service.SignInAsync(request);
        if (!authResponse.Success)
            return Unauthorized(authResponse);

        if (request.RefreshTokenAsHttpOnly)
            SetRefreshTokenInCookies(authResponse.Data!.RefreshToken);
        authResponse.Data!.RefreshToken = "";

        _logger.LogInformation("User ({email}) signed in", request.Email);

        return Ok(authResponse);
    }


    [HttpPost("refresh")]
    public async Task<ActionResult<Result<AuthResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        if (!Request.Cookies.ContainsKey("RefreshToken"))
            return Unauthorized("Invalid RefreshToken");


        var authResponse = await _service.RefreshTokenAsync(Request.Cookies["RefreshToken"]!);
        if (!authResponse.Success)
            return Unauthorized(authResponse);

        if (request.RefreshTokenAsHttpOnly)
            SetRefreshTokenInCookies(authResponse.Data!.RefreshToken);

        return Ok(authResponse);
    }


    private void SetRefreshTokenInCookies(string refreshToken)
    {
        Response.Cookies.Append("RefreshToken", refreshToken,
           new CookieOptions
           {
               HttpOnly = true,
               Path = "/api/RefreshToken",
               SameSite = SameSiteMode.Strict,
               Expires = DateTime.UtcNow.AddDays(_options.RefreshTokenExpiry)
               //Secure = true,

           });
    }
}

public class SignInRequestWithFlag : SignInRequest
{
    public bool RefreshTokenAsHttpOnly { get; set; }
}

public record RefreshTokenRequest(bool RefreshTokenAsHttpOnly);