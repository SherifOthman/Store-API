namespace OnlineStore.Application.Responses;
public class UserResponse
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; } 
    public string Role { get; set; } = string.Empty;
}
