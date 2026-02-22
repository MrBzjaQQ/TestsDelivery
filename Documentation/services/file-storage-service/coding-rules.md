# File Storage Service - Coding Rules

## Naming Conventions

```csharp
// Classes: PascalCase
public class FileUploadService : IFileUploadService

// Interfaces: IPascalCase
public interface IFileUploadService

// Methods: PascalCase
public async Task<UploadResponseDto> UploadFileAsync(...)

// Private fields: camelCase with _
private readonly IFileRepository _fileRepository;
```

## File Validation

```csharp
// ✅ Correct validation
public FileValidationResult Validate(IFormFile file)
{
    if (file.Length > _settings.MaxFileSizeMB * 1024 * 1024)
    {
        return new FileValidationResult(false, $"File too large. Max size: {_settings.MaxFileSizeMB}MB");
    }

    if (!_settings.AllowedTypes.Contains(file.ContentType))
    {
        return new FileValidationResult(false, $"File type not allowed: {file.ContentType}");
    }

    return new FileValidationResult(true, null);
}

// ❌ Incorrect - don't trust file extension only
if (file.FileName.EndsWith(".jpg")) // Can be fooled!
```

## Error Handling

```csharp
public async Task<File> GetByIdAsync(Guid id, CancellationToken ct)
{
    var file = await _repository.GetByIdAsync(id, ct);
    
    if (file == null)
    {
        throw new FileNotFoundException(id);
    }
    
    return file;
}
```

## Performance Guidelines

```csharp
// ✅ Stream files efficiently
public async Task<File> GetFileByIdAsync(Guid id, CancellationToken ct)
{
    var file = await _context.Files.FindAsync(id, ct);
    if (file == null) throw new FileNotFoundException(id);
    
    return file; // Return entity with binary data
}

// ❌ Don't load entire file if streaming
// Use Npgsql's GetStream for large files
```

## Commit Messages

```
feat(storage): add file upload endpoint
fix(image): handle transparency in PNG files
docs(api): update file upload API documentation
test(service): add unit tests for FileUploadService
refactor(processor): optimize image resizing
```