using IdentityService.Application.DTOs.Responses;
using IdentityService.Domain.ValueObjects;

namespace IdentityService.Application.Contracts;

public interface IUserService
{
    Task<UserDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken);

    Task<UserDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

    Task<RoleAssignmentResponse> AssignRoleAsync(string userId, UserRole role, CancellationToken cancellationToken);

    Task<bool> VerifyEmailAsync(string token, CancellationToken cancellationToken);
}
