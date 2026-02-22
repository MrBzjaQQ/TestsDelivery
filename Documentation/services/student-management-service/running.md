# Student Management Service - Running

## Build

```bash
dotnet build src/StudentManagementService
```

## Run Locally

```bash
cd src/StudentManagementService/StudentManagementService.WebApi

# development
dotnet run --configuration Development

# production
dotnet run --configuration Release
```

## Docker

```bash
# Build
docker build -f StudentManagementService.WebApi/Dockerfile . -t student-management-service:1.0

# Run
docker run -d \
  --name student-management-service \
  -p 8083:8080 \
  -e ConnectionStrings__DefaultConnection="Host=pg-student;Port=5432;Database=testsdelivery_students;Username=postgres;Password=postgres" \
  -e RabbitMQ__Host=rabbitmq \
  student-management-service:1.0
```

## Docker Compose

```yaml
services:
  student-management-service:
    image: student-management-service:1.0
    ports:
      - "8083:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Host=pg-student;Port=5432;Database=testsdelivery_students;Username=postgres;Password=postgres
      - RabbitMQ__Host=rabbitmq
      - ExternalServices__IdentityServiceUrl=http://identity-service:8081
      - ExternalServices__TestCheckingServiceUrl=http://test-checking-service:8084
    depends_on:
      - pg-student
      - rabbitmq
```

## Health Checks

```bash
curl http://localhost:8083/health
curl http://localhost:8083/quickhealth
```

## Logging

```bash
# Console (Development)
dotnet run --configuration Development

# Docker logs
docker logs student-management-service
```
