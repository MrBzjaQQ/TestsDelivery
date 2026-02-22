# Test Checking Service - Database Schema

## Entity-Relationship Diagram

```
┌──────────────────────┐      ┌──────────────────────┐
│   Tests              │      │   Students           │
├──────────────────────┤      ├──────────────────────┤
│ Id (PK) GUID         │      │ Id (PK) GUID         │
│ Title VARCHAR(200)   │      │ UserId GUID          │
│ PassPercentage TINY  │      │ FirstName VARCHAR    │
│ MaxScore SMALLINT    │      │ LastName VARCHAR     │
└──────────┬───────────┘      └──────────────────────┘
           │
           │ 1:N
           │
┌──────────▼───────────┐
│   TestResults        │
├──────────────────────┤
│ Id (PK) GUID         │
│ TestId (FK) GUID     │
│ StudentId (FK) GUID  │
│ Score SMALLINT       │
│ MaxScore SMALLINT    │
│ Percentage DECIMAL   │
│ IsPassed BOOLEAN     │
│ AttemptNumber TINY   │
│ PassedDate TIMESTAMP │
│ Answers JSONB        │
│ CreatedAt TIMESTAMP  │
└──────────────────────┘
```

## Database Tables

### tests

```sql
CREATE TABLE tests (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    title VARCHAR(200) NOT NULL,
    pass_percentage TINYINT NOT NULL DEFAULT 70 CHECK (pass_percentage BETWEEN 0 AND 100),
    max_score SMALLINT NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now())
);

CREATE INDEX IX_tests_title ON tests(title);
```

### students (Reference from Identity Service)

```sql
-- Students are managed in Identity Service
-- Foreign key reference from test_results
```

### test_results

```sql
CREATE TABLE test_results (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    test_id UUID NOT NULL REFERENCES tests(id) ON DELETE CASCADE,
    student_id UUID NOT NULL,
    score SMALLINT NOT NULL,
    max_score SMALLINT NOT NULL,
    percentage DECIMAL(5, 2) NOT NULL,
    is_passed BOOLEAN NOT NULL,
    attempt_number TINYINT NOT NULL DEFAULT 1,
    passed_date TIMESTAMP WITH TIME ZONE,
    answers JSONB NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now()),
    UNIQUE(test_id, student_id, attempt_number)
);

CREATE INDEX IX_test_results_test_id ON test_results(test_id);
CREATE INDEX IX_test_results_student_id ON test_results(student_id);
CREATE INDEX IX_test_results_created_at ON test_results(created_at);
```

## Migrations

```bash
cd src/TestCheckingService/TestCheckingService.Infrastructure.Database/
dotnet ef migrations add Create_TestResultTable \
  --project TestCheckingService.Infrastructure.Database.csproj \
  --startup-project ../TestCheckingService.WebApi/TestCheckingService.WebApi.csproj
```

## Database Context

### ITestCheckingDbContext

```csharp
public interface ITestCheckingDbContext : IDisposable
{
    DbSet<Test> Tests { get; set; }
    DbSet<TestResult> TestResults { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

### TestCheckingDbContext

```csharp
public class TestCheckingDbContext : DbContext, ITestCheckingDbContext
{
    public TestCheckingDbContext(DbContextOptions<TestCheckingDbContext> options)
        : base(options)
    {
    }

    public DbSet<Test> Tests { get; set; }
    public DbSet<TestResult> TestResults { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TestCheckingDbContext).Assembly);
    }
}
```

## Entity Configurations

### TestResultConfiguration

```csharp
public class TestResultConfiguration : IEntityTypeConfiguration<TestResult>
{
    public void Configure(EntityTypeBuilder<TestResult> builder)
    {
        builder.ToTable("test_results");
        
        builder.HasKey(tr => tr.Id);
        builder.Property(tr => tr.Id).HasDefaultValueSql("gen_random_uuid()");
        
        builder.Property(tr => tr.Score).IsRequired();
        builder.Property(tr => tr.MaxScore).IsRequired();
        builder.Property(tr => tr.Percentage).IsRequired().HasColumnType("DECIMAL(5,2)");
        builder.Property(tr => tr.IsPassed).IsRequired();
        builder.Property(tr => tr.AttemptNumber).IsRequired().HasDefaultValue(1);
        builder.Property(tr => tr.Answers).HasColumnType("JSONB").IsRequired();
        
        builder.HasIndex(tr => tr.TestId);
        builder.HasIndex(tr => tr.StudentId);
        builder.HasIndex(tr => tr.CreatedAt);
        
        builder.HasCheckConstraint("CK_test_results_percentage_range", 
            "percentage >= 0 AND percentage <= 100");
    }
}
```

## Indexes Summary

| Table | Column(s) | Purpose |
|-------|-----------|---------|
| test_results | test_id | Get results by test |
| test_results | student_id | Get results by student |
| test_results | created_at | Sort by submission time |
| tests | title | Search tests by title |

## Data Types

| PostgreSQL Type | EF Core Type | C# Type | Usage |
|-----------------|--------------|---------|-------|
| UUID | Guid | Guid | Primary keys |
| VARCHAR(n) | string | string | Short strings |
| SMALLINT | short | short | Test scores |
| TINYINT | byte | byte | Constrained small numbers |
| DECIMAL(5,2) | decimal | decimal | Percentage |
| BOOLEAN | bool | bool | Pass/fail |
| JSONB | string | string | Answers JSON |
| TIMESTAMP WITH TIME ZONE | DateTimeOffset | DateTimeOffset | Timestamps |

## Constraints

### Not Null Constraints

- `test_results.score` - Score is required
- `test_results.max_score` - Max score is required
- `test_results.percentage` - Percentage is required
- `test_results.answers` - Answers are required

### Check Constraints

- `percentage >= 0 AND percentage <= 100`
- `pass_percentage >= 0 AND pass_percentage <= 100`
- `attempt_number >= 1 AND attempt_number <= 3`

### Unique Constraints

- `UNIQUE(test_id, student_id, attempt_number)` - One result per attempt
