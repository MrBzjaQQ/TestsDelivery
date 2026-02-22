# Test Checking Service - Running & Deployment

## Build

```bash
dotnet build src/TestCheckingService
```

## Run Locally

```bash
cd src/TestCheckingService/TestCheckingService.WebApi

dotnet run --configuration Development
```

## Docker

```bash
docker build -f TestCheckingService.WebApi/Dockerfile . -t test-checking-service:1.0
docker run -d -p 8084:8080 test-checking-service:1.0
```

## Docker Compose

```yaml
services:
  test-checking-service:
    image: test-checking-service:1.0
    ports:
      - "8084:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Host=pg-result;Port=5432;Database=testsdelivery_results;Username=postgres;Password=postgres
      - RabbitMQ__Host=rabbitmq
      - ExternalServices__StudentServiceUrl=http://student-service:8083
      - ExternalServices__QuestionServiceUrl=http://question-service:8082
    depends_on:
      - pg-result
      - rabbitmq
```

## Health Checks

```bash
curl http://localhost:8084/health
curl http://localhost:8084/quickhealth
```