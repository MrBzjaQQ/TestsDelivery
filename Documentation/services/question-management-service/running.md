# Question Management Service - Running & Deployment

## Build

### dotnet CLI

```bash
# Build solution
dotnet build TestsDelivery.sln

# Build specific service
dotnet build src/QuestionManagementService/QuestionManagementService.sln

# Build with release configuration
dotnet build --configuration Release
```

### Docker Build

```bash
cd src/QuestionManagementService

# Build image
docker build -f QuestionManagementService.WebApi/Dockerfile . -t question-management-service:1.0

# Build with specific tag
docker build -f QuestionManagementService.WebApi/Dockerfile . -t question-management-service:test
```

## Run Locally

### Prerequisites

- .NET 10 SDK
- PostgreSQL 18.1 (or Docker with PostgreSQL)
- RabbitMQ 3.13

### Environment Setup

1. **Create database:**
```bash
docker run -d \
  --name pg-question \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=testsdelivery_questions \
  -p 5432:5432 \
  postgres:18.1
```

2. **Start RabbitMQ:**
```bash
docker run -d \
  --name rabbitmq \
  -p 5672:5672 \
  -p 15672:15672 \
  rabbitmq:3.13-management
```

### Run with dotnet CLI

```bash
cd src/QuestionManagementService/QuestionManagementService.WebApi

# Restore dependencies
dotnet restore

# Build
dotnet build

# Run (development mode)
dotnet run --configuration Development

# Or run with specific profile
dotnet run --launch-profile Development
```

### Run with Visual Studio

1. Set `QuestionManagementService.WebApi` as startup project
2. Select `Development` profile
3. Press F5

## Run in Docker

### Single Service

```bash
# Start PostgreSQL
docker run -d \
  --name pg-question \
  -e POSTGRES_PASSWORD=postgres \
  -e POSTGRES_DB=testsdelivery_questions \
  -p 5432:5432 \
  postgres:18.1

# Run service
docker run -d \
  --name question-management-service \
  -p 8082:8080 \
  -e ConnectionStrings__DefaultConnection="Host=host.docker.internal;Port=5432;Database=testsdelivery_questions;Username=postgres;Password=postgres" \
  -e RabbitMQ__Host=host.docker.internal \
  question-management-service:1.0
```

### With Docker Compose

```yaml
# docker-compose.yml
version: '3.8'

services:
  question-management-service:
    image: question-management-service:1.0
    container_name: question-management-service
    ports:
      - "8082:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Host=pg-question;Port=5432;Database=testsdelivery_questions;Username=postgres;Password=postgres
      - RabbitMQ__Host=rabbitmq
      - IdentityServiceUrl=http://identity-service:8081
    depends_on:
      - pg-question
      - rabbitmq
      - identity-service
    networks:
      - testsdelivery-network

  pg-question:
    image: postgres:18.1
    container_name: pg-question
    environment:
      - POSTGRES_PASSWORD=postgres
      - POSTGRES_DB=testsdelivery_questions
    ports:
      - "5432:5432"
    volumes:
      - question-db-data:/var/lib/postgresql/data
    networks:
      - testsdelivery-network

  rabbitmq:
    image: rabbitmq:3.13-management
    container_name: rabbitmq
    ports:
      - "5672:5672"
      - "15672:15672"
    volumes:
      - rabbitmq-data:/var/lib/rabbitmq
    networks:
      - testsdelivery-network

networks:
  testsdelivery-network:
    driver: bridge

volumes:
  question-db-data:
  rabbitmq-data:
```

### Docker Compose Commands

```bash
# Build and start all services
docker-compose up --build

# Start services
docker-compose up -d

# Stop services
docker-compose down

# View logs
docker-compose logs -f question-management-service

# Restart service
docker-compose restart question-management-service

# Run with specific profile
docker-compose --profile test up
```

## Health Checks

### Check Service Health

```bash
# Quick health check
curl http://localhost:8082/quickhealth

# Full health check
curl http://localhost:8082/health
```

### Health Check Response

**Healthy:**
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:00.0123456",
  "entries": {
    "PostgreSqlHealthCheck": {
      "status": "Healthy",
      "description": "PostgreSQL DB Query Succeeded",
      "duration": "00:00:00.0056789"
    }
  }
}
```

**Unhealthy:**
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

## Logging

### Console Logs

```bash
# Development
dotnet run --configuration Development

# Logs show:
# [10:30:00 INF] Question created successfully
# [10:30:01 WRN] Database connection retry attempt 2
# [10:30:02 ERR] Failed to process message: QuestionBankNotFoundException
```

### Serilog Output

**Console:**
```
[10:30:00 INF] [0HM29KJ2P093] [abc123...] QuestionService - Question created: a1b2c3d4...
```

**RabbitMQ (LogExchange):**
```json
{
  "Timestamp": "2024-12-01T10:30:00.000Z",
  "Level": "Information",
  "RequestId": "0HM29KJ2P093",
  "TraceId": "abc123...",
  "Message": "Question created successfully",
  "Service": "question-management-service"
}
```

## Environment Variables

### Development

```bash
export APP_NAME="question-management-service:dev"
export Db__DefaultConnection="Host=localhost;Port=5432;Database=testsdelivery_questions_dev;Username=postgres;Password=postgres"
export RabbitMQ__Host="localhost"
export RabbitMQ__Port="5672"
export IdentityServiceUrl="http://localhost:8081"
export FileStorageServiceUrl="http://localhost:8085"
```

### Production

```bash
export APP_NAME="question-management-service:prod"
export Db__DefaultConnection="Host=pg-question;Port=5432;Database=testsdelivery_questions;Username=${DB_USER};Password=${DB_PASSWORD}"
export RabbitMQ__Host="rabbitmq"
export RabbitMQ__Port="5672"
export IdentityServiceUrl="http://identity-service:8081"
export FileStorageServiceUrl="http://file-storage-service:8085"
export LOG_LEVEL="Information"
```

## Database Migrations

### Create Migration

```bash
cd src/QuestionManagementService/QuestionManagementService.Infrastructure.Database/

dotnet ef migrations add Create_QuestionTable \
  --project ../QuestionManagementService.Infrastructure.Database/QuestionManagementService.Infrastructure.Database.csproj \
  --startup-project ../QuestionManagementService.WebApi/QuestionManagementService.WebApi.csproj
```

### Apply Migration

```bash
# Automatic (on application start)
dotnet run

# Manual
dotnet ef database update \
  --project ../QuestionManagementService.Infrastructure.Database/QuestionManagementService.Infrastructure.Database.csproj \
  --startup-project ../QuestionManagementService.WebApi/QuestionManagementService.WebApi.csproj
```

### Remove Migration

```bash
dotnet ef migrations remove \
  --project ../QuestionManagementService.Infrastructure.Database/QuestionManagementService.Infrastructure.Database.csproj \
  --startup-project ../QuestionManagementService.WebApi/QuestionManagementService.WebApi.csproj
```

## API Testing

### With curl

```bash
# Get questions
curl -X GET "http://localhost:8082/api/v1/questions" \
  -H "Authorization: Bearer <token>"

# Create question
curl -X POST "http://localhost:8082/api/v1/questions" \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "text": "Test question",
    "category": "Math",
    "difficulty": 2,
    "questionBankId": "bank-id"
  }'

# Health check
curl -X GET "http://localhost:8082/health"
```

### With Postman

```json
// Environment variables
{
  "url": "http://localhost:8082",
  "token": ""
}

// Request: GET /api/v1/questions
Authorization: Bearer {{token}}
```

### With OpenAPI / Swagger

```bash
# Access Swagger UI
open http://localhost:8082/swagger
```

## Docker Compose Network

Services communicate via Docker network:

```bash
# Check network
docker network inspect testsdelivery-network

# Test connectivity
docker exec -it question-management-service ping rabbitmq
docker exec -it question-management-service ping pg-question
```

## CI/CD Build Pipeline

### GitLab CI

```yaml
stages:
  - build
  - test
  - deploy

variables:
  IMAGE_NAME: question-management-service
  DOCKER_TLS_CERTDIR: "/certs"

build:
  stage: build
  script:
    - docker login -u gitlab-ci-token -p $CI_JOB_TOKEN $CI_REGISTRY
    - docker build -f QuestionManagementService.WebApi/Dockerfile . -t $IMAGE_NAME:$CI_COMMIT_SHA
    - docker tag $IMAGE_NAME:$CI_COMMIT_SHA $CI_REGISTRY_IMAGE/$IMAGE_NAME:$CI_COMMIT_SHA
    - docker push $CI_REGISTRY_IMAGE/$IMAGE_NAME:$CI_COMMIT_SHA
  only:
    - master
    - develop

test:
  stage: test
  script:
    - dotnet restore
    - dotnet build
    - dotnet test --no-build
  artifacts:
    reports:
      junit: test-results.xml
  only:
    - merge_requests

deploy:
  stage: deploy
  script:
    - docker-compose pull
    - docker-compose up -d
  environment:
    name: production
  only:
    - master
```

## Troubleshooting

### Service won't start

**Check logs:**
```bash
docker logs question-management-service
```

**Common issues:**
- Database connection failures
- RabbitMQ connection failures
- Missing environment variables

### Database migrations fail

```bash
# Remove failed migration
dotnet ef migrations remove

# Reapply
dotnet ef database update
```

### Port already in use

```bash
# Change port in appsettings.json
"Kestrel": {
  "Endpoints": {
    "Http": {
      "Url": "http://*:8082"
    }
  }
}
```

## Monitoring

### Metrics (Prometheus)

```bash
# Access metrics endpoint
curl http://localhost:8082/metrics

# Sample metrics:
# question_service_questions_created_total 100
# question_service_http_requests_duration_seconds 0.012
# question_service_db_connections_active 5
```

### Tracing (OpenTelemetry)

```bash
# Traces are exported to OTLP endpoint
# View in Jaeger or Tempo
```

## Security

### Environment Variables (Production)

```bash
# JWT Secret (strong random string)
export JWT__SECRET_KEY="$(openssl rand -base64 32)"

# Database password
export Db__DefaultConnection="Host=...;Password=${DB_PASSWORD}"
```

### HTTPS

```csharp
// In Program.cs
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
```

## Scaling

### Horizontal Scaling

```yaml
# docker-compose.scale.yml
services:
  question-management-service:
    deploy:
      replicas: 3
    restart: on-failure
```

### Load Balancer

```nginx
# nginx.conf
upstream question_service {
    server question-service-1:8080;
    server question-service-2:8080;
    server question-service-3:8080;
}

server {
    listen 80;
    
    location /api/v1/questions {
        proxy_pass http://question_service;
    }
}
```
