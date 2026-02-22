using System.Text.Json.Serialization;

namespace IdentityService.Application.DTOs.Requests;

public class RefreshTokenRequest
{
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = string.Empty;
}
