# Bff Portal Service - Coding Rules

## Naming Conventions

```csharp
// Classes: PascalCase
public class StudentPortalService : IStudentPortalService

// Interfaces: IPascalCase
public interface IStudentPortalService

// Methods: PascalCase
public async Task<GetStudentProfileResponse> GetStudentProfileAsync(...)

// Private fields: camelCase with _
private readonly IHttpClientFactory _httpClientFactory;
```

## Error Handling

```csharp
// ✅ Aggregate errors
try
{
    var studentProfile = await _studentService.GetStudentAsync(studentId, token, ct);
    var testResults = await _testCheckingService.GetResultsAsync(studentId, token, ct);
    var groupInfo = await _groupService.GetGroupAsync(student.GroupId, token, ct);
    
    return new StudentProfileDto
    {
        Profile = studentProfile,
        Results = testResults,
        Group = groupInfo
    };
}
catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.ServiceUnavailable)
{
    throw new ServiceUnavailableException("One or more services are temporarily unavailable", ex);
}

// ❌ Don't swallow exceptions
try { /* operation */ } catch { return null; } // Silent failure!
```

## HTTP Client Usage

```csharp
// ✅ Use named clients with configuration
var client = _httpClientFactory.CreateClient("StudentService");
client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

// ❌ Don't create HttpClient directly
using var client = new HttpClient(); // Disposes after each request - performance issue!
```

## Cache Patterns

```csharp
// ✅ Proper caching
var cacheKey = $"student-profile:{studentId}";
if (_cache.TryGetValue(cacheKey, out StudentProfileDto cached))
{
    return cached;
}

var profile = await _studentService.GetProfileAsync(studentId, token, ct);
_cache.Set(cacheKey, profile, TimeSpan.FromMinutes(5));
return profile;

// ❌ Don't cache all errors
catch { return null; } // Should rethrow or return error response
```

## Commit Messages

```
feat(portal): add group analytics endpoint
fix(cache): fix cache invalidation on test submission
docs(api): update portal API documentation
test(service): add unit tests for StudentPortalService
refactor(cache): optimize cache key generation
```