# Docker Compose - TestsDelivery

## Complete docker-compose.yml

```yaml
version: '3.8'

services:
  # PostgreSQL Databases
  pg-identity:
    image: postgres:18.1
    container_name: pg-identity
    environment:
      - POSTGRES_PASSWORD=${DB_PASSWORD}
      - POSTGRES_DB=testsdelivery_identity
    volumes:
      - pg-identity-data:/var/lib/postgresql/data
    networks:
      - testsdelivery-network

  pg-question:
    image: postgres:18.1
    container_name: pg-question
    environment:
      - POSTGRES_PASSWORD=${DB_PASSWORD}
      - POSTGRES_DB=testsdelivery_questions
    volumes:
      - pg-question-data:/var/lib/postgresql/data
    networks:
      - testsdelivery-network

  pg-student:
    image: postgres:18.1
    container_name: pg-student
    environment:
      - POSTGRES_PASSWORD=${DB_PASSWORD}
      - POSTGRES_DB=testsdelivery_students
    volumes:
      - pg-student-data:/var/lib/postgresql/data
    networks:
      - testsdelivery-network

  pg-result:
    image: postgres:18.1
    container_name: pg-result
    environment:
      - POSTGRES_PASSWORD=${DB_PASSWORD}
      - POSTGRES_DB=testsdelivery_results
    volumes:
      - pg-result-data:/var/lib/postgresql/data
    networks:
      - testsdelivery-network

  pg-file:
    image: postgres:18.1
    container_name: pg-file
    environment:
      - POSTGRES_PASSWORD=${DB_PASSWORD}
      - POSTGRES_DB=testsdelivery_files
    volumes:
      - pg-file-data:/var/lib/postgresql/data
    networks:
      - testsdelivery-network

  pg-notification:
    image: postgres:18.1
    container_name: pg-notification
    environment:
      - POSTGRES_PASSWORD=${DB_PASSWORD}
      - POSTGRES_DB=testsdelivery_notifications
    volumes:
      - pg-notification-data:/var/lib/postgresql/data
    networks:
      - testsdelivery-network

  # RabbitMQ
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

  # Microservices
  identity-service:
    image: identity-service:1.0
    container_name: identity-service
    ports:
      - "8081:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Host=pg-identity;Port=5432;Database=testsdelivery_identity;Username=postgres;Password=${DB_PASSWORD}
      - RabbitMQ__Host=rabbitmq
      - RabbitMQ__Port=5672
      - RabbitMQ__Username=guest
      - RabbitMQ__Password=guest
    depends_on:
      - pg-identity
      - rabbitmq
    networks:
      - testsdelivery-network

  question-management-service:
    image: question-management-service:1.0
    container_name: question-management-service
    ports:
      - "8082:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Host=pg-question;Port=5432;Database=testsdelivery_questions;Username=postgres;Password=${DB_PASSWORD}
      - RabbitMQ__Host=rabbitmq
      - IdentityServiceUrl=http://identity-service:8081
    depends_on:
      - pg-question
      - rabbitmq
      - identity-service
    networks:
      - testsdelivery-network

  student-management-service:
    image: student-management-service:1.0
    container_name: student-management-service
    ports:
      - "8083:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Host=pg-student;Port=5432;Database=testsdelivery_students;Username=postgres;Password=${DB_PASSWORD}
      - RabbitMQ__Host=rabbitmq
      - ExternalServices__IdentityServiceUrl=http://identity-service:8081
      - ExternalServices__TestCheckingServiceUrl=http://test-checking-service:8084
    depends_on:
      - pg-student
      - rabbitmq
      - identity-service
      - test-checking-service
    networks:
      - testsdelivery-network

  test-checking-service:
    image: test-checking-service:1.0
    container_name: test-checking-service
    ports:
      - "8084:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Host=pg-result;Port=5432;Database=testsdelivery_results;Username=postgres;Password=${DB_PASSWORD}
      - RabbitMQ__Host=rabbitmq
    depends_on:
      - pg-result
      - rabbitmq
    networks:
      - testsdelivery-network

  file-storage-service:
    image: file-storage-service:1.0
    container_name: file-storage-service
    ports:
      - "8085:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Host=pg-file;Port=5432;Database=testsdelivery_files;Username=postgres;Password=${DB_PASSWORD}
      - RabbitMQ__Host=rabbitmq
    depends_on:
      - pg-file
      - rabbitmq
    networks:
      - testsdelivery-network

  notification-service:
    image: notification-service:1.0
    container_name: notification-service
    ports:
      - "8086:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Host=pg-notification;Port=5432;Database=testsdelivery_notifications;Username=postgres;Password=${DB_PASSWORD}
      - RabbitMQ__Host=rabbitmq
      - SMTP__Host=${SMTP_HOST}
      - SMTP__Port=${SMTP_PORT}
      - SMTP__Username=${SMTP_USERNAME}
      - SMTP__Password=${SMTP_PASSWORD}
    depends_on:
      - pg-notification
      - rabbitmq
    networks:
      - testsdelivery-network

  # BFF Portal
  bff-portal-service:
    image: bff-portal-service:1.0
    container_name: bff-portal-service
    ports:
      - "8080:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Host=pg-bff;Port=5432;Database=testsdelivery_bff;Username=postgres;Password=${DB_PASSWORD}
      - RabbitMQ__Host=rabbitmq
      - ServiceUrls__IdentityService=http://identity-service:8081
      - ServiceUrls__QuestionService=http://question-management-service:8082
      - ServiceUrls__StudentService=http://student-management-service:8083
      - ServiceUrls__TestCheckingService=http://test-checking-service:8084
    depends_on:
      - identity-service
      - question-management-service
      - student-management-service
      - test-checking-service
    networks:
      - testsdelivery-network

networks:
  testsdelivery-network:
    driver: bridge

volumes:
  pg-identity-data:
  pg-question-data:
  pg-student-data:
  pg-result-data:
  pg-file-data:
  pg-notification-data:
  rabbitmq-data:
```

## Environment Variables

Create `.env` file:

```bash
DB_PASSWORD=your-strong-password-here
SMTP_HOST=smtp.yourdomain.com
SMTP_PORT=587
SMTP_USERNAME=your-smtp-username
SMTP_PASSWORD=your-smtp-password
```

## Building Images

```bash
# Build all services
docker-compose build

# Build specific service
docker-compose build question-management-service
```

## Running

```bash
# Start all services
docker-compose up -d

# Start and follow logs
docker-compose up --build

# Stop services
docker-compose down

# Stop and remove volumes
docker-compose down -v
```

## Access Points

| Service | URL | Port |
|---------|-----|------|
| Swagger UI (BFF) | http://localhost:8080/swagger | 8080 |
| Swagger UI (Question) | http://localhost:8082/swagger | 8082 |
| Swagger UI (Student) | http://localhost:8083/swagger | 8083 |
| RabbitMQ Management | http://localhost:15672 | 15672 |
| PostgreSQL | localhost:5432 | 5432 |
