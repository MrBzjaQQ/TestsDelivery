# Question Management Service - Configuration

## Application Settings

### appsettings.json

```json
{
  "AppName": "question-management-service:test",
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=testsdelivery_questions;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "SecretKey": "question-management-service-secret-key-2024-change-in-production",
    "Issuer": "TestsDelivery",
    "Audience": "TestsDelivery",
    "ExpirationMinutes": 60
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Port": 5672,
    "Username": "guest",
    "Password": "guest",
    "QueueNames": {
      "TestCreatedQueue": "question-test-created",
      "QuestionBankUpdatedQueue": "question-bank-updated"
    },
    "ExchangeName": "question-management-exchange"
  },
  "IdentityServiceUrl": "http://identity-service:8081",
  "FileStorageServiceUrl": "http://file-storage-service:8085",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    },
    "Serilog": {
      "MinimumLevel": {
        "Default": "Information",
        "Override": {
          "Microsoft": "Warning",
          "System": "Warning"
        }
      },
      "WriteTo": [
        {
          "Name": "RabbitMQ",
          "Args": {
            "username": "guest",
            "password": "guest",
            "hostnames": [ "localhost" ],
            "port": 5672,
            "exchange": "LogExchange",
            "autoCreateExchange": true,
            "batchPostingLimit": 1000,
            "exchangeType": "direct",
            "routingKey": "question-management",
            "bufferingTimeLimit": "0.00:00:02.00",
            "deliveryMode": "Durable"
          }
        },
        {
          "Name": "Console"
        }
      ]
    }
  },
  "OpenTelemetry": {
    "ServiceName": "question-management-service",
    "OtlpEndpoint": "http://localhost:4317",
    "ActivitySourceName": "question-management-activity"
  },
  "HealthChecks": {
    "Enabled": true,
    "DatabaseTimeoutSeconds": 5
  }
}
```

### appsettings.Development.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=testsdelivery_questions_dev;Username=postgres;Password=postgres"
  },
  "RabbitMQ": {
    "Host": "localhost",
    "Port": 5672
  },
  "IdentityServiceUrl": "http://localhost:8081",
  "FileStorageServiceUrl": "http://localhost:8085",
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Debug"
    }
  }
}
```

### appsettings.Production.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=pg-question;Port=5432;Database=testsdelivery_questions;Username=${DB_USER};Password=${DB_PASSWORD}"
  },
  "RabbitMQ": {
    "Host": "rabbitmq",
    "Port": 5672
  },
  "IdentityServiceUrl": "http://identity-service:8081",
  "FileStorageServiceUrl": "http://file-storage-service:8085",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

## Configuration Options

### AppSettings.cs

```csharp
public class AppSettings
{
    public string AppName { get; set; }
    public string AllowedHosts { get; set; }
    
    public JwtSettings Jwt { get; set; }
    public RabbitMQSettings RabbitMQ { get; set; }
    
    public string IdentityServiceUrl { get; set; }
    public string FileStorageServiceUrl { get; set; }
    
    public HealthCheckSettings HealthChecks { get; set; }
}

public class JwtSettings
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audiences { get; set; }
    public int ExpirationMinutes { get; set; }
}

public class RabbitMQSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    
    public QueueNames QueueNames { get; set; }
    public string ExchangeName { get; set; }
}

public class QueueNames
{
    public string TestCreatedQueue { get; set; }
    public string QuestionBankUpdatedQueue { get; set; }
}

public class HealthCheckSettings
{
    public bool Enabled { get; set; }
    public int DatabaseTimeoutSeconds { get; set; }
}
```

## Connection String Format

### PostgreSQL Connection Strings

**Development (localhost):**
```
Host=localhost;Port=5432;Database=testsdelivery_questions_dev;Username=postgres;Password=postgres
```

**Production (Docker):**
```
Host=pg-question;Port=5432;Database=testsdelivery_questions;Username=${DB_USER};Password=${DB_PASSWORD}
```

**Production (External):**
```
Host=your-server.com;Port=5432;Database=testsdelivery_questions;Username=app_user;Password=strong_password;Pooling=true;Min Pool Size=5;Max Pool Size=20;Connection Lifetime=300;
```

## Environment Variables

| Variable | Required | Description |
|----------|----------|-------------|
| `APP_NAME` | No | Application name (defaults to `question-management-service`) |
| `Db__DefaultConnection` | Yes | PostgreSQL connection string |
| `RabbitMQ__Host` | Yes | RabbitMQ host (default: `localhost`) |
| `RabbitMQ__Port` | No | RabbitMQ port (default: `5672`) |
| `RabbitMQ__Username` | Yes | RabbitMQ username (default: `guest`) |
| `RabbitMQ__Password` | Yes | RabbitMQ password (default: `guest`) |
| `IdentityServiceUrl` | Yes | URL of IdentityService |
| `FileStorageServiceUrl` | Yes | URL of FileStorageService |

## RabbitMQ Configuration

### Exchange: question-management-exchange

| Setting | Value |
|---------|-------|
| Type | direct |
| Durable | true |
| AutoDelete | false |

### Queues

| Queue | Binding Key | Consumer |
|-------|-------------|----------|
| question-test-created | test.created | TestCreatedConsumer |
| question-bank-updated | bank.updated | QuestionBankUpdatedConsumer |

## Logging Configuration

### Serilog Levels

| Level | Description | Usage |
|-------|-------------|-------|
| `Verbose` | Detailed trace | Development only |
| `Debug` | Debug messages | Development |
| `Information` | General info | Production |
| `Warning` | Warnings | Production |
| `Error` | Errors | Production |
| `Fatal` | Critical failures | Production |

### Log Message Format

```
{Timestamp} [{Level}] {RequestId} {TraceId} {Message}{NewLine}{Exception}
```

Example:
```
2024-12-01 10:30:00.000 +00:00 [Information] 0HM29KJ2P093 00-abc123def456... Question created successfully
```

## Health Check Configuration

### Database Health Check

**Endpoint:** `/health` or `/quickhealth`

**Check Name:** `PostgreSqlHealthCheck`

**Query:** `SELECT 1`

**Timeout:** 5 seconds

### Health Check Response

**Healthy (200):**
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.0123456"
}
```

**Unhealthy (503):**
```json
{
  "status": "Unhealthy",
  "totalDuration": "00:00:05.0012345",
  "entries": {
    "PostgreSqlHealthCheck": {
      "status": "Unhealthy",
      "description": "PostgreSQL DB Query Failed",
      "duration": "00:00:05.0000000"
    }
  }
}
```

## OpenTelemetry Configuration

### Metrics

| Metric | Description |
|--------|-------------|
| `http.server.duration` | Request duration |
| `dotnet.runtime` | .NET runtime metrics |
| `dotnet.threads` | Thread pool metrics |

### Traces

- HTTP requests/responses
- Database operations
- RabbitMQ messages

## Database Configuration

### EF Core Connection Options

```csharp
services.AddNpgsql<QuestionDbContext>(connectionString, options =>
{
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.UseLazyLoadingProxies(false);
    options.UsePostgresGIS(); // If using GIS
});
```

### Connection Pooling

```csharp
options.UseNpgsql(connectionString, npgsqlOptions =>
{
    npgsqlOptions.ConnectionIdleTimeout = 30;
    npgsqlOptions.Pooling = true;
    npgsqlOptions.MaxPoolSize = 20;
    npgsqlOptions.MinPoolSize = 5;
});
```

## JWT Authentication Configuration

### Token Generation Settings

| Setting | Recommended Value |
|---------|-------------------|
| SecretKey | 256-bit random string |
| Issuer | Your domain (e.g., `testsdelivery.com`) |
| Audience | Client domain |
| ExpirationMinutes | 60 (short-lived) |

### JWT Structure

```json
{
  "header": {
    "alg": "HS256",
    "typ": "JWT"
  },
  "payload": {
    "sub": "user-id",
    "role": "Admin",
    "scope": "question:read question:write",
    "iat": 1701426600,
    "exp": 1701430200
  }
}
```
