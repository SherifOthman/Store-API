using System.Text.Json.Serialization;

namespace OnlineStore.Application.Responses;

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    
    [JsonIgnore]
    public string RefreshToken { get; set; } = string.Empty;
}