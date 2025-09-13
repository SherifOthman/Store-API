using OnlineStore.Application.Providers;

namespace OnlineStore.Api.utils;


public class LoggedInUser : ILoggedInUser
{

    private readonly IHttpContextAccessor _accessor;

    public LoggedInUser(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string GetIpAddress()
    {
        var ip = _accessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

        var forwarded = _accessor.HttpContext?.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        return forwarded ?? ip ?? "Unknown";
    }

    public int GetUserId()
    {
       var userIdClaim = _accessor.HttpContext?.User.FindFirst("sub")?.Value;

        return userIdClaim != null ? int.Parse(userIdClaim) : 0;
    }
}