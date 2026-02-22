using FluentAssertions;
using IdentityService.Application.DTOs.Responses;
using IdentityService.Domain.Exceptions;
using IdentityService.Domain.ValueObjects;
using IdentityService.Infrastructure.Database.Identity;
using IdentityService.Infrastructure.Database.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace IdentityService.Tests.Unit.Services;

public class UserServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;

    public UserServiceTests()
    {
        var userStore = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            userStore.Object, null, null, null, null, null, null, null, null);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldThrowUserNotFoundException_WhenUserNotFound()
    {
        _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser?)null);

        var userService = new UserService(_mockUserManager.Object);

        var act = async () => await userService.GetUserByIdAsync("non-existent-id", CancellationToken.None);

        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUserDto_WhenUserFound()
    {
        var user = new ApplicationUser
        {
            Id = "user-id",
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Role = "Student",
            EmailVerified = true,
            CreatedAt = DateTime.UtcNow,
        };

        _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        _mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string> { "Student" });

        var userService = new UserService(_mockUserManager.Object);

        var result = await userService.GetUserByIdAsync("user-id", CancellationToken.None);

        result.Should().NotBeNull();
        result.Id.Should().Be("user-id");
        result.Email.Should().Be("test@example.com");
        result.FirstName.Should().Be("John");
        result.LastName.Should().Be("Doe");
        result.Role.Should().Be(UserRole.Student);
    }

    [Fact]
    public async Task AssignRoleAsync_ShouldThrowUserNotFoundException_WhenUserNotFound()
    {
        _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((ApplicationUser?)null);

        var userService = new UserService(_mockUserManager.Object);

        var act = async () => await userService.AssignRoleAsync("non-existent-id", UserRole.Teacher, CancellationToken.None);

        await act.Should().ThrowAsync<UserNotFoundException>();
    }

    [Fact]
    public async Task AssignRoleAsync_ShouldReturnRoleAssignmentResponse_WhenRoleAssigned()
    {
        var user = new ApplicationUser
        {
            Id = "user-id",
            Email = "test@example.com",
            Role = "Student",
        };

        _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        _mockUserManager.Setup(um => um.GetRolesAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(new List<string> { "Student" });
        _mockUserManager.Setup(um => um.RemoveFromRolesAsync(It.IsAny<ApplicationUser>(), It.IsAny<IEnumerable<string>>()))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);
        _mockUserManager.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        var userService = new UserService(_mockUserManager.Object);

        var result = await userService.AssignRoleAsync("user-id", UserRole.Teacher, CancellationToken.None);

        result.Should().NotBeNull();
        result.UserId.Should().Be("user-id");
        result.Role.Should().Be(UserRole.Teacher);
    }
}
