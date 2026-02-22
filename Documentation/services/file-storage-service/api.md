# File Storage Service - API Reference

## Base URL

```
http://localhost:8085/api/v1
```

## Responses

All API responses follow the `ResponseResultModel<T>` pattern.

## Files API

### Upload File

Uploads a file to storage.

**Endpoint:** `POST /api/v1/files/upload`

**Request (multipart/form-data):**
```
Content-Type: multipart/form-data

File: <binary>
Metadata: {
  "ownerId": "user-id-here",
  "type": "question-image" | "document"
}
```

**Success Response (201):**
```json
{
  "isError": false,
  "timestamp": "2024-12-01T10:30:00Z",
  "message": "File uploaded successfully",
  "data": {
    "id": "f1i2l3e4-s5t6o7r8-e9a0g1e2-s3t4o5r6",
    "fileName": "question-image.jpg",
    "contentType": "image/jpeg",
    "size": 256000,
    "uploadedAt": "2024-12-01T10:30:00Z",
    "ownerId": "user-id-here"
  }
}
```

**Validation Rules:**
- Maximum file size: 10MB
- Allowed image types: JPEG, PNG, GIF, WebP
- Allowed document types: PDF, DOC, DOCX
- Image dimensions: max 1920x1080

### Get File

Retrieves a file by ID.

**Endpoint:** `GET /api/v1/files/{id}`

**Path Parameters:**
- `id` (string, required): File ID

**Success Response (200):**
```
Content-Type: image/jpeg
Content-Length: 256000

[Binary file data]
```

**Headers:**
```
Content-Disposition: inline; filename="question-image.jpg"
Cache-Control: public, max-age=86400
```

### Get Thumbnail

Gets image thumbnail (200x200).

**Endpoint:** `GET /api/v1/files/{id}/thumbnail`

**Success Response (200):**
```
Content-Type: image/jpeg
Content-Length: 50000

[Thumbnail binary data]
```

### Delete File

Deletes a file.

**Endpoint:** `DELETE /api/v1/files/{id}`

**Success Response (204):** No content

**Error Responses:**
- `404` - File not found

### Get File Metadata

Gets file metadata without downloading.

**Endpoint:** `GET /api/v1/files/{id}/metadata`

**Success Response (200):**
```json
{
  "isError": false,
  "message": "File metadata retrieved",
  "data": {
    "id": "file-id-here",
    "fileName": "question-image.jpg",
    "contentType": "image/jpeg",
    "size": 256000,
    "uploadedAt": "2024-12-01T10:30:00Z",
    "ownerId": "user-id-here",
    "width": 1920,
    "height": 1080
  }
}
```

## HTTP Status Codes

| Code | Description |
|------|-------------|
| `200` | OK - Request succeeded |
| `201` | Created - File uploaded |
| `204` | No Content - DELETE succeeded |
| `400` | Bad Request - Invalid file |
| `401` | Unauthorized - Missing/invalid token |
| `403` | Forbidden - Insufficient permissions |
| `404` | Not Found - File not found |
| `413` | Payload Too Large - File too big |
| `500` | Internal Server Error |

## Error Models

### FileNotFoundException

```json
{
  "type": "FileNotFoundException",
  "title": "An error occurred while processing your request",
  "status": 404,
  "detail": "File with id '123' not found",
  "instance": "GET: /api/v1/files/123",
  "traceId": "00-abc123..."
}
```

### InvalidFileException

```json
{
  "type": "InvalidFileException",
  "title": "An error occurred while processing your request",
  "status": 400,
  "detail": "File type not allowed or file too large",
  "instance": "POST: /api/v1/files/upload",
  "traceId": "00-abc123...",
  "violations": [
    {
      "field": "file",
      "message": "Maximum file size is 10MB",
      "code": "FileSizeExceeded"
    }
  ]
}
```

## Example: Upload Question Image

### 1. Upload Image

```bash
POST /api/v1/files/upload
Content-Type: multipart/form-data

File: [binary image data]
Metadata: {
  "ownerId": "teacher-id",
  "type": "question-image"
}
```

### 2. Response

```json
{
  "id": "image-id-here",
  "fileName": "question1.jpg",
  "contentType": "image/jpeg",
  "size": 125000
}
```

### 3. Use in Question

```bash
POST /api/v1/questions
{
  "text": "Identify the landmark in the image",
  "imageFileId": "image-id-here",
  "options": [...]
}
```