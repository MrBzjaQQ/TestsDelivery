using System.Text.Json.Serialization;
using IdentityService.Domain.ValueObjects;

namespace IdentityService.Application.DTOs.Responses;

public class UserDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("role")]
    public UserRole Role { get; set; }

    [JsonPropertyName("emailVerified")]
    public bool EmailVerified { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
}
