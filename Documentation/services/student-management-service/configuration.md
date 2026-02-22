# Student Management Service - Configuration

## AppSettings

```json
{
  "AppName": "student-management-service:test",
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=testsdelivery_students;Username=postgres;Password=postgres"
  },
  "Jwt": {
    "SecretKey": "student-management-secret",
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
      "TestResultReceivedQueue": "student-test-result",
      "TestAssignedQueue": "student-test-assigned"
    },
    "ExchangeName": "student-management-exchange"
  },
  "ExternalServices": {
    "IdentityServiceUrl": "http://identity-service:8081",
    "TestCheckingServiceUrl": "http://test-checking-service:8084",
    "FileStorageServiceUrl": "http://file-storage-service:8085"
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
            "routingKey": "student-management"
          }
        }
      ]
    }
  },
  "OpenTelemetry": {
    "ServiceName": "student-management-service",
    "OtlpEndpoint": "http://localhost:4317"
  },
  "HealthChecks": {
    "Enabled": true,
    "DatabaseTimeoutSeconds": 5
  }
}
```

## External Service Configuration

### IdentityService
```json
"ExternalServices": {
  "IdentityServiceUrl": "http://identity-service:8081"
}
```

### TestCheckingService
```json
"ExternalServices": {
  "TestCheckingServiceUrl": "http://test-checking-service:8084"
}
```

### FileStorageService
```json
"ExternalServices": {
  "FileStorageServiceUrl": "http://file-storage-service:8085"
}
```

## RabbitMQ Queues

| Queue | Binding Key | Consumer |
|-------|-------------|----------|
| student-test-result | test.result.received | TestResultReceivedConsumer |
| student-test-assigned | test.assigned | TestAssignedConsumer |
