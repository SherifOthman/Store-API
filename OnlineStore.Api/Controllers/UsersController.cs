using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Application.Common;
using OnlineStore.Application.Requests;
using OnlineStore.Application.Responses;

namespace OnlineStore.Api.Controllers;
[Route("api/users")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<ActionResult<Result<UserResponse>>> GetCurrentUser()
    {
        var result = await _userService.GetLoggedInUserAsync();

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    [Authorize]
    [HttpPut("me")]
    public async Task<IActionResult> UpdateCurrentUser(UpdateUserRequest request)
    {

        var result = await _userService.UpdateLoggedInUserAsync(request);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return NoContent();
    }
}
