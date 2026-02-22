# Notification Service - Configuration

## AppSettings

```json
{
  "AppName": "notification-service:test",
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=testsdelivery_notifications;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "SecretKey": "notification-secret",
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
      "TestCreatedQueue": "notification-test-created",
      "TestResultQueue": "notification-test-result",
      "UserRegisteredQueue": "notification-user-registered"
    },
    "ExchangeName": "notification-exchange"
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
  "EmailTemplates": {
    "BasePath": "Templates",
    "DefaultLanguage": "en"
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
            "routingKey": "notification"
          }
        }
      ]
    }
  },
  "OpenTelemetry": {
    "ServiceName": "notification-service",
    "OtlpEndpoint": "http://localhost:4317"
  },
  "HealthChecks": {
    "Enabled": true,
    "DatabaseTimeoutSeconds": 5
  }
}
```

## SmtpSettings

```csharp
public class SmtpSettings
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string From { get; set; }
    public string DisplayName { get; set; }
    public bool EnableSsl { get; set; }
}
```

## RabbitMQ Configuration

### Exchange: notification-exchange

| Setting | Value |
|---------|-------|
| Type | fanout |
| Durable | true |
| AutoDelete | false |

### Queues

| Queue | Binding Key | Consumer |
|-------|-------------|----------|
| notification-test-created | test.created | TestCreatedConsumer |
| notification-test-result | test.result.received | TestResultConsumer |
| notification-user-registered | user.registered | UserRegisteredConsumer |

## Email Templates

### Template Files

```
Templates/
├── en/
│   ├── test-created.hbs
│   ├── test-result.hbs
│   └── user-registered.hbs
└── ru/
    ├── test-created.hbs
    ├── test-result.hbs
    └── user-registered.hbs
```

### Template Rendering

```csharp
// Use Handlebars.Net
var template = File.ReadAllText("Templates/en/test-created.hbs");
var handlebars = HandlebarsDotNet.Handlebars.RegisterHelper("if", (writer, context, args) =>
{
    // Handlebars if helper
});
var result = handlebars.Compile(template)(new { testTitle = "Math" });
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

### SMTP Health Check

```csharp
// Check SMTP connectivity
public class SmtpHealthCheck : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(...)
    {
        try
        {
            using var client = new SmtpClient(_settings.Host, _settings.Port);
            await client.ConnectAsync();
            await client.DisconnectAsync(true);
            return HealthCheckResult.Healthy("SMTP connection successful");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("SMTP connection failed", ex);
        }
    }
}
```

## OpenTelemetry Configuration

### Metrics

| Metric | Description |
|--------|-------------|
| `notification_service.emails_sent_total` | Emails sent count |
| `notification_service.failed_emails_total` | Failed emails count |
| `notification_service.template_render_duration_seconds` | Template rendering time |

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