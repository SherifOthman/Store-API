namespace OnlineStore.Application.DTOs;
public record UserDto(
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    string? AvatarUrl,
    int Roles,
    DateTime CreatedAt);
