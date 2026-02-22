# File Storage Service - Running & Deployment

## Build

```bash
dotnet build src/FileStorageService
```

## Run Locally

```bash
cd src/FileStorageService/FileStorageService.WebApi
dotnet run --configuration Development
```

## Docker

```bash
docker build -f FileStorageService.WebApi/Dockerfile . -t file-storage-service:1.0
docker run -d -p 8085:8080 file-storage-service:1.0
```

## Docker Compose

```yaml
services:
  file-storage-service:
    image: file-storage-service:1.0
    ports:
      - "8085:8080"
    environment:
      - ConnectionStrings__DefaultConnection=Host=pg-file;Port=5432;Database=testsdelivery_files;Username=postgres;Password=postgres
    depends_on:
      - pg-file
```

## Health Checks

```bash
curl http://localhost:8085/health
curl http://localhost:8085/quickhealth
```

## Example: Upload Question Image

### cURL Example

```bash
curl -X POST "http://localhost:8085/api/v1/files/upload" \
  -H "Authorization: Bearer <token>" \
  -F "file=@question-image.jpg" \
  -F 'metadata={"ownerId":"teacher-id","type":"question-image"}'
```

### Response

```json
{
  "isError": false,
  "data": {
    "id": "image-id-here",
    "fileName": "question-image.jpg",
    "size": 125000,
    "contentType": "image/jpeg"
  }
}
```

### Use in Question

```bash
POST /api/v1/questions
{
  "text": "Identify the landmark",
  "imageFileId": "image-id-here",
  "options": [...]
}
```