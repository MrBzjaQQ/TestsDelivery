# File Storage Service - Configuration

## AppSettings

```json
{
  "AppName": "file-storage-service:test",
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=testsdelivery_files;Username=postgres;Password=postgres"
  },
  "FileStorage": {
    "MaxFileSizeMB": 10,
    "AllowedImageTypes": [ "image/jpeg", "image/png", "image/gif", "image/webp" ],
    "AllowedDocumentTypes": [ "application/pdf", "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" ],
    " ThumbnailWidth": 200,
    "ThumbnailHeight": 200,
    "MaxImageWidth": 1920,
    "MaxImageHeight": 1080,
    "JpegQuality": 85,
    "ThumbnailQuality": 75
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    },
    "Serilog": {
      "MinimumLevel": "Information",
      "WriteTo": [ { "Name": "Console" } ]
    }
  },
  "OpenTelemetry": {
    "ServiceName": "file-storage-service",
    "OtlpEndpoint": "http://localhost:4317"
  },
  "HealthChecks": {
    "Enabled": true,
    "DatabaseTimeoutSeconds": 5
  }
}
```

## FileStorageSettings

```csharp
public class FileStorageSettings
{
    public int MaxFileSizeMB { get; set; }
    public List<string> AllowedImageTypes { get; set; }
    public List<string> AllowedDocumentTypes { get; set; }
    public int ThumbnailWidth { get; set; }
    public int ThumbnailHeight { get; set; }
    public int MaxImageWidth { get; set; }
    public int MaxImageHeight { get; set; }
    public int JpegQuality { get; set; }
    public int ThumbnailQuality { get; set; }
}
```

## Database Schema

```sql
CREATE TABLE files (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    file_name VARCHAR(255) NOT NULL,
    content_type VARCHAR(100) NOT NULL,
    size BIGINT NOT NULL,
    data BYTEA NOT NULL,
    owner_id UUID NOT NULL,
    width SMALLINT,
    height SMALLINT,
    is_image BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now())
);

CREATE INDEX IX_files_owner_id ON files(owner_id);
CREATE INDEX IX_files_created_at ON files(created_at);
```

## File Types

| Type | Extensions | Max Size | Notes |
|------|-----------|----------|-------|
| Images | .jpg, .jpeg, .png, .gif, .webp | 10MB | Auto-resize to 1920x1080 |
| Documents | .pdf, .doc, .docx | 10MB | No processing |
| Thumbnails | - | - | Auto-generated for images |

## Health Checks

### Database Health Check

**Endpoint:** `/health`

**Check Name:** `PostgreSqlHealthCheck`

**Query:** `SELECT 1`

## Logging

### Log Messages

```json
{
  "Timestamp": "2024-12-01T10:30:00.000Z",
  "Level": "Information",
  "Message": "File uploaded: {FileName}, Size: {Size} bytes",
  "Service": "file-storage-service"
}
```