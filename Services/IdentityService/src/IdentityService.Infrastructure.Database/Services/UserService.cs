using IdentityService.Application.Contracts;
using IdentityService.Application.DTOs.Responses;
using IdentityService.Domain.Exceptions;
using IdentityService.Domain.ValueObjects;
using IdentityService.Infrastructure.Database.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Database.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserDto> GetUserByIdAsync(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? user.Role;

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = Enum.Parse<UserRole>(role),
            EmailVerified = user.EmailVerified,
            CreatedAt = user.CreatedAt,
        };
    }

    public async Task<UserDto> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user == null)
        {
            throw new UserNotFoundException(email, true);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? user.Role;

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = Enum.Parse<UserRole>(role),
            EmailVerified = user.EmailVerified,
            CreatedAt = user.CreatedAt,
        };
    }

    public async Task<RoleAssignmentResponse> AssignRoleAsync(string userId, UserRole role, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            throw new UserNotFoundException(userId);
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, role.ToString());

        user.Role = role.ToString();
        await _userManager.UpdateAsync(user);

        return new RoleAssignmentResponse
        {
            UserId = userId,
            Role = role,
            AssignedAt = DateTime.UtcNow,
        };
    }

    public async Task<bool> VerifyEmailAsync(string token, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u => u.Email == token, cancellationToken);

        if (user == null)
        {
            return false;
        }

        user.EmailVerified = true;
        user.EmailConfirmed = true;
        await _userManager.UpdateAsync(user);

        return true;
    }
}
