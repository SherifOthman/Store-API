

using OnlineStore.Domain.Enums;

namespace OnlineStore.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; } = string.Empty;
   
    // Generated and retrived by Enum with bitwise operator
    public RoleValue Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }


}
