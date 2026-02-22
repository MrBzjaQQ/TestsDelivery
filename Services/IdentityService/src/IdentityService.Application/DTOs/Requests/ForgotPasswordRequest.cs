using System.Text.Json.Serialization;

namespace IdentityService.Application.DTOs.Requests;

public class ForgotPasswordRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}
