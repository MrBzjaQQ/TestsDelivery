# Student Management Service - Coding Rules

## Naming Conventions

```csharp
// Classes: PascalCase
public class StudentService : IStudentService

// Interfaces: IPascalCase
public interface IStudentService

// Methods: PascalCase
public async Task<CreateStudentResponse> CreateAsync(...)

// Private fields: camelCase with _
private readonly IStudentRepository _studentRepository;
```

## Error Handling

```csharp
public async Task<Student> GetByIdAsync(Guid id, CancellationToken ct)
{
    var student = await _repository.GetByIdAsync(id, ct);
    
    if (student == null)
    {
        throw new StudentNotFoundException(id);
    }
    
    return student;
}
```

## Commit Messages

```
feat(student): add student registration endpoint
fix(test): handle test submission errors
docs(api): update student API documentation
refactor(service): optimize progress tracking
test(controller): add unit tests for StudentsController
```
