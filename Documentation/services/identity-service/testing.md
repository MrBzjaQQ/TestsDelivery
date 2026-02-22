# Identity Service - Testing

## Test Structure

```
IdentityService.Tests/
├── Unit/
│   ├── Services/
│   │   ├── AuthServiceTests.cs
│   │   ├── TokenServiceTests.cs
│   │   └── UserServiceTests.cs
│   └── Controllers/
│       ├── AuthControllerTests.cs
│       └── UsersControllerTests.cs
└── Integration/
    ├── TestInfrastructure/
    │   ├── DatabaseFixture.cs
    │   └── DbTestsBase.cs
    └── Controllers/
        └── AuthControllerTests.cs
```

## Unit Tests

### AuthServiceTests

```csharp
public class AuthServiceTests
{
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly AuthService _service;

    [Fact]
    public async Task Login_Should_ReturnTokens_When_Valid_Credentials()
    {
        // Arrange
        var user = new ApplicationUser { Id = "user-id", Email = "test@example.com" };
        _mockUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync(user);
        _mockSignInManager.Setup(sm => sm.CheckPasswordSignInAsync(user, "Password123!", false))
            .ReturnsAsync(SignInResult.Success);
        _mockTokenService.Setup(ts => ts.GenerateTokensAsync(user))
            .ReturnsAsync(new AuthResponse { AccessToken = "token", RefreshToken = "refresh" });

        _service = new AuthService(_mockUserManager.Object, _mockSignInManager.Object, _mockTokenService.Object);

        // Act
        var result = await _service.LoginAsync("test@example.com", "Password123!");

        // Assert
        result.IsError.Should().BeFalse();
        result.Data.AccessToken.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Login_Should_ThrowInvalidCredentialsException_When_Invalid()
    {
        // Arrange
        _mockUserManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>(), CancellationToken.None))
            .ReturnsAsync((ApplicationUser)null);

        _service = new AuthService(_mockUserManager.Object, _mockSignInManager.Object, _mockTokenService.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
            _service.LoginAsync("test@example.com", "WrongPassword"));
    }
}
```

### TokenServiceTests

```csharp
public class TokenServiceTests
{
    private readonly TokenService _service;

    [Fact]
    public void GenerateTokensAsync_Should_ReturnValidTokens()
    {
        // Arrange
        var user = new ApplicationUser { Id = "user-id", Email = "test@example.com" };
        _service = CreateService();

        // Act
        var result = _service.GenerateTokensAsync(user, CancellationToken.None).Result;

        // Assert
        result.AccessToken.Should().NotBeNullOrEmpty();
        result.RefreshToken.Should().NotBeNullOrEmpty();
        result.ExpiresIn.Should().Be(60);
    }
}
```

## Integration Tests

### DatabaseFixture

```csharp
public class DatabaseFixture : IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:18.1")
        .Build();

    public PostgreSqlContainer DbContainer => _dbContainer;

    public async Task InitializeAsync() => await DbContainer.StartAsync();
    public async Task DisposeAsync() => await DbContainer.DisposeAsync();
}
```

### AuthControllerTests (Integration)

```csharp
[Collection("Database")]
public class AuthControllerTests : DbTestsBase
{
    private readonly AuthController _controller;

    public AuthControllerTests(DatabaseFixture fixture) : base(fixture)
    {
        _controller = new AuthController(
            new AuthService(new UserManager<ApplicationUser>(/* ... */)));
    }

    [Fact]
    public async Task Login_Should_ReturnTokens_With_Database()
    {
        // Arrange
        await SeedTestDataAsync();

        var loginRequest = new LoginRequest
        {
            Email = "student@university.com",
            Password = "SecurePassword123!"
        };

        // Act
        var result = await _controller.Login(loginRequest, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Data.AccessToken.Should().NotBeNullOrEmpty();
    }
}
```

## UserRegistration Tests

```csharp
public class UserServiceTests
{
    [Fact]
    public async Task RegisterUser_Should_CreateUser_With_Role()
    {
        // Arrange
        var user = new ApplicationUser
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Role = "Student"
        };

        _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _service = new UserService(_mockUserManager.Object, _mockRoleManager.Object);

        // Act
        var result = await _service.RegisterUserAsync(new RegisterRequest
        {
            Email = "test@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe",
            Role = "Student"
        }, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
    }
}
```

## Running Tests

```bash
dotnet test
dotnet test --filter "FullyQualifiedName~Unit"
dotnet test --filter "FullyQualifiedName~Integration"
```