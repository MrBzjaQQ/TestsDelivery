using System.Text.Json.Serialization;
using IdentityService.Domain.ValueObjects;

namespace IdentityService.Application.DTOs.Responses;

public class RoleAssignmentResponse
{
    [JsonPropertyName("userId")]
    public string UserId { get; set; } = string.Empty;

    [JsonPropertyName("role")]
    public UserRole Role { get; set; }

    [JsonPropertyName("assignedAt")]
    public DateTime AssignedAt { get; set; }
}
