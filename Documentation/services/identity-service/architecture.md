# Identity Service - Architecture

## Структура проекта

### Слой Domain

```
IdentityService.Domain/
├── Entities/
│   ├── User.cs                        # ASP.NET Core Identity User
│   └── Role.cs                        # ASP.NET Core Identity Role
├── ValueObjects/
│   ├── UserRole.cs                    # User role enum
│   ├── PasswordHash.cs                | Password hash
│   └── EmailVerificationToken.cs      | Token for email verification
└── Exceptions/
    ├── UserNotFoundException.cs
    ├── InvalidCredentialsException.cs
    └── DuplicateEmailException.cs
```

### Слой Application

```
IdentityService.Application/
├── Services/
│   ├── UserService.cs
│   ├── AuthService.cs
│   ├── TokenService.cs
│   └── EmailVerificationService.cs
├── DTOs/
│   ├── Requests/
│   │   ├── RegisterRequest.cs
│   │   ├── LoginRequest.cs
│   │   ├── RefreshTokenRequest.cs
│   │   └── ForgotPasswordRequest.cs
│   └── Responses/
│       ├── AuthResponse.cs
│       ├── UserDto.cs
│       └── TokenResponse.cs
├── Specifications/
│   ├── UserSpecs/
│   │   ├── EmailNotExistsSpecification.cs
│   │   └── UserExistsSpecification.cs
│   └── TokenSpecs/
│       ├── TokenValidSpecification.cs
│       └── TokenNotExpiredSpecification.cs
├── Contracts/
│   ├── IUserService.cs
│   ├── IAuthService.cs
│   └── ITokenService.cs
└── Exceptions/
    └── TokenRefreshException.cs
```

### Слой Infrastructure

```
IdentityService.Infrastructure/
├── Database/
│   ├── Context/
│   │   ├── IdentityDbContext.cs
│   │   └── IIdentityDbContext.cs
│   ├── Migrations/
│   │   ├── 20241201000000_Create_IdentityTables.cs
│   │   └── IdentityDbContextModelSnapshot.cs
│   └── Migrator/
│       └── DatabaseMigrator.cs
├── Repositories/
│   ├── GenericRepository.cs
│   ├── UnitOfWork.cs
│   └── UserRepository.cs
├── Identity/
│   ├── ApplicationUser.cs             # Custom User
│   ├── ApplicationRole.cs             # Custom Role
│   ├── EmailVerificationToken.cs
│   └── RefreshToken.cs
├── MassTransit/
│   ├── Consumer/
│   │   └── UserRegisteredConsumer.cs
│   └── MessageTypes/
│       └── UserRegisteredEvent.cs
└── JWT/
    ├── TokenOptions.cs
    └── TokenProvider.cs
```

### Слой WebApi

```
IdentityService.WebApi/
├── Controllers/
│   ├── AuthController.cs
│   ├── UsersController.cs
│   └── RolesController.cs
├── Settings/
│   ├── AppSettings.cs
│   ├── JwtSettings.cs
│   └── IdentitySettings.cs
├── Shared/
│   └── ResponseResultModel.cs
├── HealthChecks/
│   └── PostgreSqlHealthCheck.cs
├── Handler/
│   └── CustomExceptionHandler.cs
├── Factories/
│   ├── ProblemDetailsFactory.cs
│   └── IProblemDetailsFactory.cs
├── Constants/
│   ├── ProblemDetailsConstants.cs
│   └── LogMessageConstants.cs
├── DependencyInjection.cs
└── Program.cs
```

### Слой Tests

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

### Layer Flow

```
Client Request
    ↓
WebApi (Controller)
    ↓
Application (Service)
    ↓
Application (Specification)
    ↓
Infrastructure (Repository/Identity Manager)
    ↓
PostgreSQL
    ↓
Response
```

## Dependency Injection

```csharp
public static class DependencyInjection
{
    public static WebApplicationBuilder ConfigureBuilder(this WebApplicationBuilder builder, AppSettings settings)
    {
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        
        // Identity
        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<IdentityDbContext>()
            .AddDefaultTokenProviders();
        
        // Database
        builder.Services.AddDatabase(settings.ConnectionString);
        
        // JWT
        builder.Services.AddJwtAuthentication(builder.Configuration);
        
        // Services
        builder.Services.AddApplicationServices();
        
        // MassTransit
        builder.Services.AddMassTransitWithRabbitMq(builder.Configuration);
        
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<CustomExceptionHandler>();
        
        builder.Services.AddHealthChecks()
            .AddCheck<PostgreSqlHealthCheck>("PostgreSqlHealthCheck");
        
        return builder;
    }
}
```

## Patterns Implementation

### JWT Token Service

```csharp
public class TokenService : ITokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public async Task<AuthResponse> GenerateTokensAsync(ApplicationUser user)
    {
        var tokenOptions = new TokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes)
        };

        var jwtToken = _tokenHandler.CreateToken(tokenOptions);
        var refreshToken = GenerateRefreshToken();
        
        await SaveRefreshTokenAsync(user.Id, refreshToken);
        
        return new AuthResponse
        {
            AccessToken = _tokenHandler.WriteToken(jwtToken),
            RefreshToken = refreshToken.Token,
            ExpiresIn = (int)_jwtSettings.ExpirationMinutes
        };
    }

    private RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddHours(24),
            Created = DateTime.UtcNow,
            CreatedByIp = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString()
        };
        
        return refreshToken;
    }
}
```

### MassTransit Consumer

```csharp
public class UserRegisteredConsumer : IConsumer<UserRegisteredEvent>
{
    private readonly IEmailNotificationService _emailService;
    private readonly ILogger<UserRegisteredConsumer> _logger;

    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        var @event = context.Message;
        
        _logger.LogInformation("Processing user registered notification for {Email}", @event.Email);
        
        var sendRequest = new SendEmailRequest
        {
            To = @event.Email,
            Subject = "Welcome to TestsDelivery!",
            TemplateName = "user-registered",
            TemplateData = new { username = @event.Email }
        };

        await _emailService.SendEmailAsync(sendRequest, context.CancellationToken);
    }
}
```

## Folder Structure Summary

| Folder | Purpose | Content |
|--------|---------|---------|
| **Domain** | Business entities | Entities, ValueObjects, Exceptions |
| **Application** | Business logic | Services, DTOs, Specifications |
| **Infrastructure** | Data access | DbContext, Repositories, Identity, JWT |
| **WebApi** | API layer | Controllers, Settings, HealthChecks |
| **Tests** | Tests | Unit and Integration tests |