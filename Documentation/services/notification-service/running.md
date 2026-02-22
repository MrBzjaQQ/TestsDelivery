# Notification Service - Running & Deployment

## Build

```bash
dotnet build src/NotificationService
```

## Run Locally

```bash
cd src/NotificationService/NotificationService.WebApi
dotnet run --configuration Development
```

## Docker

```bash
docker build -f NotificationService.WebApi/Dockerfile . -t notification-service:1.0
docker run -d -p 8086:8080 notification-service:1.0
```

## Docker Compose

```yaml
services:
  notification-service:
    image: notification-service:1.0
    ports:
      - "8086:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Host=pg-notification;Port=5432;Database=testsdelivery_notifications;Username=postgres;Password=postgres
      - RabbitMQ__Host=rabbitmq
      - Smtp__Host=smtp.example.com
      - Smtp__Port=587
      - Smtp__Username=noreply@example.com
      - Smtp__Password=smtp-password
    depends_on:
      - pg-notification
      - rabbitmq
```

## Health Checks

```bash
curl http://localhost:8086/health
curl http://localhost:8086/quickhealth
```

## SMTP Configuration

### Using Gmail SMTP

```json
"Smtp": {
  "Host": "smtp.gmail.com",
  "Port": 587,
  "Username": "your-email@gmail.com",
  "Password": "app-password",
  "From": "your-email@gmail.com",
  "DisplayName": "TestsDelivery",
  "EnableSsl": true
}
```

### Using MailHog (Development)

```json
"Smtp": {
  "Host": "localhost",
  "Port": 1025,
  "EnableSsl": false
}
```