# Bff Portal Service - Configuration

## AppSettings

```json
{
  "AppName": "bff-portal-service:test",
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=testsdelivery_bff;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "SecretKey": "bff-portal-secret",
    "Issuer": "TestsDelivery",
    "Audience": "TestsDelivery",
    "ExpirationMinutes": 60
  },
  "ServiceUrls": {
    "IdentityServiceUrl": "http://identity-service:8081",
    "QuestionServiceUrl": "http://question-management-service:8082",
    "StudentServiceUrl": "http://student-management-service:8083",
    "TestCheckingServiceUrl": "http://test-checking-service:8084",
    "FileStorageServiceUrl": "http://file-storage-service:8085",
    "NotificationServiceUrl": "http://notification-service:8086"
  },
  "Cache": {
    "StudentProfileDurationMinutes": 5,
    "AvailableTestsDurationMinutes": 10,
    "GroupAnalyticsDurationMinutes": 15,
    "TestQuestionsDurationMinutes": 60
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
            "routingKey": "bff-portal"
          }
        }
      ]
    }
  },
  "OpenTelemetry": {
    "ServiceName": "bff-portal-service",
    "OtlpEndpoint": "http://localhost:4317"
  },
  "HealthChecks": {
    "Enabled": true,
    "DatabaseTimeoutSeconds": 5,
    "ExternalServiceTimeoutSeconds": 3
  }
}
```

## ServiceUrls

```csharp
public class ServiceUrls
{
    public string IdentityServiceUrl { get; set; }
    public string QuestionServiceUrl { get; set; }
    public string StudentServiceUrl { get; set; }
    public string TestCheckingServiceUrl { get; set; }
    public string FileStorageServiceUrl { get; set; }
    public string NotificationServiceUrl { get; set; }
}
```

## Cache Settings

```csharp
public class CacheSettings
{
    public int StudentProfileDurationMinutes { get; set; }    // Default: 5
    public int AvailableTestsDurationMinutes { get; set; }   // Default: 10
    public int GroupAnalyticsDurationMinutes { get; set; }   // Default: 15
    public int TestQuestionsDurationMinutes { get; set; }    // Default: 60
}
```

## HTTP Client Configuration

```csharp
// In Program.cs
builder.Services.AddHttpClient("StudentService", client =>
{
    client.BaseAddress = new Uri(settings.ServiceUrls.StudentServiceUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});
```

## Health Check Configuration

### Database Health Check

**Endpoint:** `/health`

**Check Name:** `PostgreSqlHealthCheck`

**Query:** `SELECT 1`

### External Service Health Checks

**Endpoints:**
- `/health/identity` - IdentityService
- `/health/question` - QuestionService
- `/health/student` - StudentService
- `/health/test-checking` - TestCheckingService

## OpenTelemetry Configuration

### Metrics

| Metric | Description |
|--------|-------------|
| `bff_portal.requests_total` | Requests count |
| `bff_portal.external_calls_total` | External service calls |
| `bff_portal.cache_hit_ratio` | Cache hit ratio |

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

## Database Configuration

### Connection Pooling

```csharp
options.UseNpgsql(connectionString, npgsqlOptions =>
{
    npgsqlOptions.MaxPoolSize = 10;
    npgsqlOptions.MinPoolSize = 2;
    npgsqlOptions.ConnectionIdleTimeout = 30;
});
```