# Test Checking Service - Coding Rules

## Naming Conventions

```csharp
// Classes: PascalCase
public class TestCheckService : ITestCheckService

// Interfaces: IPascalCase
public interface ITestCheckService

// Methods: PascalCase
public async Task<CheckTestResponse> CheckTestAsync(...)

// Private fields: camelCase with _
private readonly ITestResultRepository _testResultRepository;
```

## Error Handling

```csharp
public async Task<TestResult> GetByIdAsync(Guid testId, Guid studentId, CancellationToken ct)
{
    var result = await _repository.GetByIdAsync(testId, studentId, ct);
    
    if (result == null)
    {
        throw new TestResultNotFoundException(testId, studentId);
    }
    
    return result;
}
```

## Scoring Logic Standards

```csharp
// ✅ Correct scoring calculation
var percentage = (double)totalScore / maxScore * 100;
var isPassed = percentage >= passThreshold;

// ❌ Incorrect - don't use integer division
var percentage = totalScore / maxScore * 100; // This would be 0!
```

## Commit Messages

```
feat(checking): add batch check tests endpoint
fix(scoring): handle division by zero
docs(api): update test checking API documentation
refactor(engine): optimize scoring algorithm
test(service): add unit tests for AnswerEvaluator
```

## Performance Guidelines

### Database

```csharp
// ✅ Use AsNoTracking for read-only operations
var questions = await _context.Questions
    .AsNoTracking()
    .Where(q => q.TestId == testId)
    .ToListAsync(ct);

// ❌ Don't use N+1 queries
foreach (var question in questions)
{
    var option = await _context.Options.FindAsync(question.OptionId); // N+1!
}
```
