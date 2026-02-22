using System.Text.Json.Serialization;
using IdentityService.Domain.ValueObjects;

namespace IdentityService.Application.DTOs.Requests;

public class RegisterRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("firstName")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("role")]
    public UserRole Role { get; set; } = UserRole.Student;
}
