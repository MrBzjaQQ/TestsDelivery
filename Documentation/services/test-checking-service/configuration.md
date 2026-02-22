# Test Checking Service - Configuration

## AppSettings

```json
{
  "AppName": "test-checking-service:test",
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=testsdelivery_results;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "SecretKey": "test-checking-secret",
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
      "TestSubmittedQueue": "test-checking-submitted",
      "TestResultReceivedQueue": "test-checking-result"
    },
    "ExchangeName": "test-checking-exchange"
  },
  "Scoring": {
    "PassPercentage": 70,
    "AllowRetries": true,
    "MaxAttempts": 3,
    "AttemptCooldownMinutes": 5
  },
  "ExternalServices": {
    "StudentServiceUrl": "http://student-service:8083",
    "QuestionServiceUrl": "http://question-service:8082",
    "NotificationServiceUrl": "http://notification-service:8086"
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
            "routingKey": "test-checking"
          }
        }
      ]
    }
  },
  "OpenTelemetry": {
    "ServiceName": "test-checking-service",
    "OtlpEndpoint": "http://localhost:4317"
  },
  "HealthChecks": {
    "Enabled": true,
    "DatabaseTimeoutSeconds": 5
  }
}
```

## Scoring Settings

### ScoringSettings.cs

```csharp
public class ScoringSettings
{
    public int PassPercentage { get; set; }           // Default: 70
    public bool AllowRetries { get; set; }            // Default: true
    public int MaxAttempts { get; set; }              // Default: 3
    public int AttemptCooldownMinutes { get; set; }   // Default: 5
}
```

### Scoring Logic

| Setting | Description |
|---------|-------------|
| `PassPercentage` | Minimum percentage to pass test (70%) |
| `AllowRetries` | Can student retake test? |
| `MaxAttempts` | Maximum retry count |
| `AttemptCooldownMinutes` | Minimum time between retries |

## RabbitMQ Configuration

### Exchange: test-checking-exchange

| Setting | Value |
|---------|-------|
| Type | direct |
| Durable | true |
| AutoDelete | false |

### Queues

| Queue | Binding Key | Consumer |
|-------|-------------|----------|
| test-checking-submitted | test.submitted | TestSubmittedConsumer |
| test-checking-result | test.result.received | TestResultReceivedConsumer |

## External Service URLs

### Student Service
```json
"ExternalServices": {
  "StudentServiceUrl": "http://student-service:8083"
}
```

### Question Service
```json
"ExternalServices": {
  "QuestionServiceUrl": "http://question-service:8082"
}
```

### Notification Service
```json
"ExternalServices": {
  "NotificationServiceUrl": "http://notification-service:8086"
}
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

### Log Messages

```json
{
  "Timestamp": "2024-12-01T10:30:00.000Z",
  "Level": "Information",
  "Message": "Checking test {TestId} for student {StudentId}",
  "Service": "test-checking-service",
  "TraceId": "abc123..."
}
```

## Health Check Configuration

### Database Health Check

**Endpoint:** `/health`

**Check Name:** `PostgreSqlHealthCheck`

**Query:** `SELECT 1`

**Timeout:** 5 seconds

## OpenTelemetry Configuration

### Metrics

| Metric | Description |
|--------|-------------|
| `test_checking.checks_total` | Total checks performed |
| `test_checking.score_average` | Average score |
| `test_checking.pass_rate` | Pass percentage |

### Traces

- HTTP requests/responses
- Database operations
- RabbitMQ messages

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