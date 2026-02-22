using System.Text.Json.Serialization;

namespace IdentityService.Application.DTOs.Requests;

public class ResetPasswordRequest
{
    [JsonPropertyName("token")]
    public string Token { get; set; } = string.Empty;

    [JsonPropertyName("newPassword")]
    public string NewPassword { get; set; } = string.Empty;
}
