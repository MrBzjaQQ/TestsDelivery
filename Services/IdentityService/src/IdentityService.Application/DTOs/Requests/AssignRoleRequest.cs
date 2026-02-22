using System.Text.Json.Serialization;
using IdentityService.Domain.ValueObjects;

namespace IdentityService.Application.DTOs.Requests;

public class AssignRoleRequest
{
    [JsonPropertyName("role")]
    public UserRole Role { get; set; }
}
