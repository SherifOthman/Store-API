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
    private readonly JwtOptions _options;

    public AuthController(IAuthService service,
        ILoggedInUser loggedInUser,
        IOptions<JwtOptions> options)
    {
        _service = service;
        _options = options.Value;
    }

    [HttpPost("SignUp")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request)
    {
        var result = await _service.SignUpAsync(request);
        if (!result.Success)
            return BadRequest(result);

        return NoContent();
    }

    [HttpPost("SignIn")]
    public async Task<ActionResult<Result<AuthResponse>>> SignIn([FromBody] SignInRequestWithFlag request)
    {
        var authResponse = await _service.SignInAsync(request);
        if (!authResponse.Success)
            return Unauthorized(authResponse);

        if (request.RefreshTokenAsHttpOnly)
            SetRefreshTokenInCookies(authResponse.Data!.RefreshToken);
        authResponse.Data!.RefreshToken = "";

        return Ok(authResponse);
    }


    [HttpPost("RefreshToken")]
    public async Task<ActionResult<Result<AuthResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
    {

        HttpContext.Connection.RemoteIpAddress?.ToString();

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