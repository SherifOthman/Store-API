
namespace OnlineStore.Domain.Entities;
public class RefreshToken
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public bool IsRevoked { get; set; } = false;
    public string Value { get; set; } = default!;
    public DateTime ExpiryDate { get; set; }

}
