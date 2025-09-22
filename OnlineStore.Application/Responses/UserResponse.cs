namespace OnlineStore.Application.Responses;
public record UserResponse(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string? AvatarUrl,
    int Roles);
