# Identity Service - Running & Deployment

## Build

```bash
dotnet build src/IdentityService
```

## Run Locally

```bash
cd src/IdentityService/IdentityService.WebApi
dotnet run --configuration Development
```

## Docker

```bash
docker build -f IdentityService.WebApi/Dockerfile . -t identity-service:1.0
docker run -d -p 8081:8080 identity-service:1.0
```

## Docker Compose

```yaml
services:
  identity-service:
    image: identity-service:1.0
    ports:
      - "8081:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Host=pg-identity;Port=5432;Database=testsdelivery_identity;Username=postgres;Password=postgres
      - RabbitMQ__Host=rabbitmq
      - Smtp__Host=smtp.example.com
      - Smtp__Port=587
      - Smtp__Username=noreply@example.com
      - Smtp__Password=smtp-password
    depends_on:
      - pg-identity
      - rabbitmq
```

## Health Checks

```bash
curl http://localhost:8081/health
curl http://localhost:8081/quickhealth
```

## JWT Secret Generation

```bash
# Generate secure 256-bit secret
openssl rand -base64 32
```

## Testing with MailHog

```json
"Smtp": {
  "Host": "localhost",
  "Port": 1025,
  "EnableSsl": false,
  "From": "noreply@testsdelivery.local"
}
```

Access MailHog UI at: http://localhost:8025