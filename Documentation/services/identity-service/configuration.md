# Identity Service - Configuration

## AppSettings

```json
{
  "AppName": "identity-service:test",
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=testsdelivery_identity;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "SecretKey": "identity-256-bit-secret-key-change-in-production-use-openssl-rand-base64-32",
    "Issuer": "TestsDelivery",
    "Audience": "TestsDelivery",
    "ExpirationMinutes": 60,
    "RefreshTokenExpirationHours": 24
  },
  "Identity": {
    "PasswordRequiredLength": 8,
    "PasswordRequireDigit": true,
    "PasswordRequireLowercase": true,
    "PasswordRequireUppercase": true,
    "PasswordRequireNonAlphanumeric": true,
    "UserRequireUniqueEmail": true,
    "EmailConfirmationRequired": true
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Port": 5672,
    "Username": "guest",
    "Password": "guest",
    "QueueNames": {
      "UserRegisteredQueue": "identity-user-registered"
    },
    "ExchangeName": "identity-exchange"
  },
  "Smtp": {
    "Host": "smtp.example.com",
    "Port": 587,
    "Username": "noreply@example.com",
    "Password": "smtp-password",
    "From": "noreply@example.com",
    "DisplayName": "TestsDelivery",
    "EnableSsl": true
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    },
    "Serilog": {
      "MinimumLevel": "Information",
      "WriteTo": [
        { "Name": "Console" },
        {
          "Name": "RabbitMQ",
          "Args": {
            "hostnames": [ "localhost" ],
            "exchange": "LogExchange",
            "routingKey": "identity"
          }
        }
      ]
    }
  },
  "OpenTelemetry": {
    "ServiceName": "identity-service",
    "OtlpEndpoint": "http://localhost:4317"
  },
  "HealthChecks": {
    "Enabled": true,
    "DatabaseTimeoutSeconds": 5
  }
}
```

## JwtSettings

```csharp
public class JwtSettings
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpirationMinutes { get; set; }
    public int RefreshTokenExpirationHours { get; set; }
}
```

## Identity Settings

```csharp
public class IdentitySettings
{
    public int PasswordRequiredLength { get; set; }
    public bool PasswordRequireDigit { get; set; }
    public bool PasswordRequireLowercase { get; set; }
    public bool PasswordRequireUppercase { get; set; }
    public bool PasswordRequireNonAlphanumeric { get; set; }
    public bool UserRequireUniqueEmail { get; set; }
    public bool EmailConfirmationRequired { get; set; }
}
```

## RabbitMQ Configuration

### Exchange: identity-exchange

| Setting | Value |
|---------|-------|
| Type | direct |
| Durable | true |
| AutoDelete | false |

### Queues

| Queue | Binding Key | Consumer |
|-------|-------------|----------|
| identity-user-registered | user.registered | UserRegisteredConsumer |

## ASP.NET Core Identity Configuration

### ApplicationUser

```csharp
public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Role { get; set; }  // Student, Teacher, Admin
    public bool EmailVerified { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation properties
    public ICollection<RefreshToken> RefreshTokens { get; set; }
}
```

### Token Validation

```csharp
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidAudience = _jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });
```

## Logging Configuration

### Log Levels

| Level | Description | Usage |
|-------|-------------|-------|
| `Verbose` | Detailed trace | Development only |
| `Debug` | Debug messages | Development |
| `Information` | General info | Production |
| `Warning` | Warnings | Production |
| `Error` | Errors | Production |
| `Fatal` | Critical | Production |

## Health Check Configuration

### Database Health Check

**Endpoint:** `/health`

**Check Name:** `PostgreSqlHealthCheck`

**Query:** `SELECT 1`

## OpenTelemetry Configuration

### Metrics

| Metric | Description |
|--------|-------------|
| `identity_service.logins_total` | Login attempts count |
| `identity_service.registered_users_total` | Registered users count |
| `identity_service.token_generation_duration_seconds` | Token generation time |

## Database Configuration

### Connection Pooling

```csharp
options.UseNpgsql(connectionString, npgsqlOptions =>
{
    npgsqlOptions.MaxPoolSize = 20;
    npgsqlOptions.MinPoolSize = 5;
    npgsqlOptions.ConnectionIdleTimeout = 30;
});
```