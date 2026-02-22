# Bff Portal Service - Running & Deployment

## Build

```bash
dotnet build src/BffPortalService
```

## Run Locally

```bash
cd src/BffPortalService/BffPortalService.WebApi
dotnet run --configuration Development
```

## Docker

```bash
docker build -f BffPortalService.WebApi/Dockerfile . -t bff-portal-service:1.0
docker run -d -p 8080:8080 bff-portal-service:1.0
```

## Docker Compose

```yaml
services:
  bff-portal-service:
    image: bff-portal-service:1.0
    ports:
      - "8080:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Host=pg-bff;Port=5432;Database=testsdelivery_bff;Username=postgres;Password=postgres
      - ServiceUrls__IdentityServiceUrl=http://identity-service:8081
      - ServiceUrls__QuestionServiceUrl=http://question-management-service:8082
      - ServiceUrls__StudentServiceUrl=http://student-management-service:8083
      - ServiceUrls__TestCheckingServiceUrl=http://test-checking-service:8084
      - ServiceUrls__FileStorageServiceUrl=http://file-storage-service:8085
      - ServiceUrls__NotificationServiceUrl=http://notification-service:8086
    depends_on:
      - pg-bff
      - identity-service
      - question-management-service
      - student-management-service
      - test-checking-service
      - file-storage-service
      - notification-service
```

## Health Checks

```bash
curl http://localhost:8080/health
curl http://localhost:8080/health/identity
curl http://localhost:8080/health/question
curl http://localhost:8080/health/student
curl http://localhost:8080/health/test-checking
```

## External Service Health Check

```csharp
public class ExternalServiceHealthCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ServiceUrls _serviceUrls;

    public async Task<HealthCheckResult> CheckHealthAsync(...)
    {
        var services = new[] { "Identity", "Question", "Student", "TestChecking" };
        var results = new Dictionary<string, HealthCheckResult>();
        
        foreach (var service in services)
        {
            try
            {
                var client = _httpClientFactory.CreateClient($"{service}Service");
                var response = await client.GetAsync("/quickhealth", ct);
                results[service] = response.IsSuccessStatusCode 
                    ? HealthCheckResult.Healthy($"{service} service healthy")
                    : HealthCheckResult.Unhealthy($"{service} service unhealthy");
            }
            catch (Exception ex)
            {
                results[service] = HealthCheckResult.Unhealthy($"Failed to connect to {service} service", ex);
            }
        }
        
        return results.Values.All(r => r.Status == HealthStatus.Healthy)
            ? HealthCheckResult.Healthy("All external services healthy")
            : HealthCheckResult.Unhealthy("Some external services unhealthy", results);
    }
}
```

## Caching Strategy

### Cache Keys

| Cache Key | Expiration | Content |
|-----------|-----------|---------|
| `student-profile:{studentId}` | 5 minutes | Student profile + stats |
| `available-tests:{userId}` | 10 minutes | Available tests list |
| `group-analytics:{groupId}` | 15 minutes | Group statistics |
| `test-questions:{testId}` | 1 hour | Test questions |

### Cache Invalidation

- On test submission
- On profile update
- On assignment change