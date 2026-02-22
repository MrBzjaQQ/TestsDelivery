# Question Management Service - Database Schema

## Entity-Relationship Diagram

```
┌─────────────────────────┐      ┌─────────────────────────┐
│   QuestionBank          │      │   TestTemplate          │
├─────────────────────────┤      ├─────────────────────────┤
│ Id (PK) GUID            │      │ Id (PK) GUID            │
│ Name VARCHAR(200)       │      │ Name VARCHAR(200)       │
│ Description TEXT        │      │ Description TEXT        │
│ OwnerId GUID            │      │ DefaultDuration INT     │
│ CreatedAt TIMESTAMP     │      │ DefaultPassingScore TINY│
│ IsDeleted BIT           │      │ Configuration JSONB     │
└────────────┬────────────┘      └────────────┬────────────┘
             │                                 │
             │ 1:N                             │ N:1
             │                                 │
┌────────────▼────────────┐      ┌────────────▼────────────┐
│   Question              │      │   Test                  │
├─────────────────────────┤      ├─────────────────────────┤
│ Id (PK) GUID            │      │ Id (PK) GUID            │
│ Text TEXT               │      │ Title VARCHAR(200)      │
│ Category VARCHAR(50)    │      │ Description TEXT        │
│ Difficulty TINYINT      │      │ QuestionBankId (FK) GUID│
│ QuestionBankId (FK) GUID│      │ TemplateId (FK) GUID    │
│ IsDeleted BIT           │      │ DurationMinutes INT     │
└────────────┬────────────┘      │ PassingScore TINYINT    │
             │                   │ MaxAttempts TINYINT     │
             │ 1:N               │ Status VARCHAR(20)      │
             │                   │ CreatedAt TIMESTAMP     │
┌────────────▼────────────┐      └─────────────────────────┘
│   AnswerOption          │
├─────────────────────────┤
│ Id (PK) GUID            │
│ QuestionId (FK) GUID    │
│ Text VARCHAR(500)       │
│ IsCorrect BIT           │
│ Ordinal INT             │
└─────────────────────────┘
```

## Database Tables

### question_banks

```sql
CREATE TABLE question_banks (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(200) NOT NULL,
    description TEXT,
    owner_id UUID NOT NULL REFERENCES identity_users(id) ON DELETE CASCADE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now()),
    is_deleted BOOLEAN DEFAULT FALSE
);

-- Indexes
CREATE INDEX IX_question_banks_owner_id ON question_banks(owner_id);
CREATE INDEX IX_question_banks_is_deleted ON question_banks(is_deleted);
CREATE INDEX IX_question_banks_name ON question_banks(name);
```

### questions

```sql
CREATE TABLE questions (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    text TEXT NOT NULL,
    category VARCHAR(50) NOT NULL,
    difficulty SMALLINT NOT NULL CHECK (difficulty BETWEEN 1 AND 3),
    question_bank_id UUID NOT NULL REFERENCES question_banks(id) ON DELETE CASCADE,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now()),
    is_deleted BOOLEAN DEFAULT FALSE
);

-- Indexes
CREATE INDEX IX_questions_question_bank_id ON questions(question_bank_id);
CREATE INDEX IX_questions_category ON questions(category);
CREATE INDEX IX_questions_difficulty ON questions(difficulty);
CREATE INDEX IX_questions_is_deleted ON questions(is_deleted);
```

### question_bank_questions (Junction table for many-to-many)

```sql
CREATE TABLE question_bank_questions (
    question_bank_id UUID NOT NULL REFERENCES question_banks(id) ON DELETE CASCADE,
    question_id UUID NOT NULL REFERENCES questions(id) ON DELETE CASCADE,
    PRIMARY KEY (question_bank_id, question_id)
);
```

### test_templates

```sql
CREATE TABLE test_templates (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(200) NOT NULL,
    description TEXT,
    default_duration SMALLINT NOT NULL DEFAULT 60,
    default_passing_score TINYINT NOT NULL DEFAULT 70,
    configuration JSONB,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now()),
    is_deleted BOOLEAN DEFAULT FALSE
);

-- Indexes
CREATE INDEX IX_test_templates_is_deleted ON test_templates(is_deleted);
CREATE INDEX IX_test_templates_name ON test_templates(name);
```

### tests

```sql
CREATE TABLE tests (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    title VARCHAR(200) NOT NULL,
    description TEXT,
    question_bank_id UUID NOT NULL REFERENCES question_banks(id) ON DELETE CASCADE,
    template_id UUID REFERENCES test_templates(id) ON DELETE SET NULL,
    duration_minutes SMALLINT NOT NULL,
    passing_score TINYINT NOT NULL DEFAULT 70,
    max_attempts TINYINT NOT NULL DEFAULT 3,
    status VARCHAR(20) NOT NULL DEFAULT 'Draft' CHECK (status IN ('Draft', 'Active', 'Archived')),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now()),
    is_deleted BOOLEAN DEFAULT FALSE
);

-- Indexes
CREATE INDEX IX_tests_question_bank_id ON tests(question_bank_id);
CREATE INDEX IX_tests_template_id ON tests(template_id);
CREATE INDEX IX_tests_status ON tests(status);
CREATE INDEX IX_tests_is_deleted ON tests(is_deleted);
```

### test_questions (Junction table)

```sql
CREATE TABLE test_questions (
    test_id UUID NOT NULL REFERENCES tests(id) ON DELETE CASCADE,
    question_id UUID NOT NULL REFERENCES questions(id) ON DELETE CASCADE,
    ordinal INT NOT NULL,
    PRIMARY KEY (test_id, question_id)
);
```

### answers

```sql
CREATE TABLE answers (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    question_id UUID NOT NULL REFERENCES questions(id) ON DELETE CASCADE,
    student_id UUID NOT NULL REFERENCES identity_users(id) ON DELETE CASCADE,
    test_id UUID NOT NULL REFERENCES tests(id) ON DELETE CASCADE,
    selected_option_id UUID REFERENCES answer_options(id) ON DELETE SET NULL,
    text_answer VARCHAR(2000),
    is_correct BOOLEAN,
    score TINYINT,
    answered_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now())
);

-- Indexes
CREATE INDEX IX_answers_test_id ON answers(test_id);
CREATE INDEX IX_answers_student_id ON answers(student_id);
CREATE INDEX IX_answers_question_id ON answers(question_id);
```

### answer_options

```sql
CREATE TABLE answer_options (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    question_id UUID NOT NULL REFERENCES questions(id) ON DELETE CASCADE,
    text VARCHAR(500) NOT NULL,
    is_correct BOOLEAN NOT NULL DEFAULT FALSE,
    ordinal INT NOT NULL
);

-- Indexes
CREATE INDEX IX_answer_options_question_id ON answer_options(question_id);
CREATE INDEX IX_answer_options_ordinal ON answer_options(ordinal);
```

## Migrations Guide

### Naming Convention

Migrations must follow this naming pattern:

| Prefix | Description | Example |
|--------|-------------|---------|
| `Create_` | New table | `Create_QuestionTable` |
| `Insert_` | Data seeding | `Insert_DefaultQuestionBanks` |
| `Delete_` | Remove data | `Delete_OldTests` |
| `Drop_` | Drop table | `Drop_TestQuestionsTable` |
| `Alter_` | Modify table | `Alter_AddedColumnToQuestions` |

### Creating Migrations

```bash
cd src/QuestionManagementService/QuestionManagementService.Infrastructure.Database/
dotnet ef migrations add Create_QuestionTable --project QuestionManagementService.Infrastructure.Database.csproj --startup-project ../QuestionManagementService.WebApi/QuestionManagementService.WebApi.csproj
```

### Removing Migrations

```bash
dotnet ef migrations remove --project QuestionManagementService.Infrastructure.Database.csproj --startup-project ../QuestionManagementService.WebApi/QuestionManagementService.WebApi.csproj
```

### Applying Migrations

**Automatic (on startup):**
```csharp
// In Program.cs
app.MigrateDatabase();
```

**Manual:**
```bash
dotnet ef database update --project QuestionManagementService.Infrastructure.Database.csproj --startup-project ../QuestionManagementService.WebApi/QuestionManagementService.WebApi.csproj
```

## Database Context

### IQuestionDbContext

```csharp
public interface IQuestionDbContext : IDisposable
{
    DbSet<Question> Questions { get; set; }
    DbSet<QuestionBank> QuestionBanks { get; set; }
    DbSet<Test> Tests { get; set; }
    DbSet<TestTemplate> TestTemplates { get; set; }
    DbSet<AnswerOption> AnswerOptions { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
```

### QuestionDbContext

```csharp
public class QuestionDbContext : DbContext, IQuestionDbContext
{
    public QuestionDbContext(DbContextOptions<QuestionDbContext> options)
        : base(options)
    {
    }

    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionBank> QuestionBanks { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<TestTemplate> TestTemplates { get; set; }
    public DbSet<AnswerOption> AnswerOptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(QuestionDbContext).Assembly);
        
        // Global query filters
        modelBuilder.Entity<Question>().HasQueryFilter(q => !q.IsDeleted);
        modelBuilder.Entity<QuestionBank>().HasQueryFilter(q => !q.IsDeleted);
        modelBuilder.Entity<Test>().HasQueryFilter(t => !t.IsDeleted);
        
        // Seed data
        SeedData(modelBuilder);
    }
}
```

## Entity Configurations

### QuestionConfiguration

```csharp
public class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("questions");
        
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id).HasDefaultValueSql("gen_random_uuid()");
        
        builder.Property(q => q.Text).IsRequired().HasColumnType("TEXT");
        builder.Property(q => q.Category).IsRequired().HasColumnType("VARCHAR(50)");
        builder.Property(q => q.Difficulty).IsRequired().HasConversion<byte>();
        
        builder.HasOne(q => q.QuestionBank)
            .WithMany(b => b.Questions)
            .HasForeignKey(q => q.QuestionBankId);
        
        builder.HasMany(q => q.AnswerOptions)
            .WithOne(ao => ao.Question)
            .HasForeignKey(ao => ao.QuestionId);
        
        builder.HasIndex(q => q.QuestionBankId);
        builder.HasIndex(q => q.Category);
        builder.HasIndex(q => q.Difficulty);
    }
}
```

## Data Types

| PostgreSQL Type | EF Core Type | C# Type | Usage |
|-----------------|--------------|---------|-------|
| `UUID` | `Guid` | `Guid` | Primary keys, foreign keys |
| `VARCHAR(n)` | `string` | `string` | Short strings |
| `TEXT` | `string` | `string` | Long text (questions) |
| `SMALLINT` | `short` | `short` | Small numbers |
| `TINYINT` | `byte` | `byte` | Constrained small numbers |
| `TIMESTAMP WITH TIME ZONE` | `DateTimeOffset` | `DateTimeOffset` | Timestamps |
| `BOOLEAN` | `bool` | `bool` | Boolean flags |
| `JSONB` | `string` | `string` | JSON data (template config) |

## Indexes Summary

### Performance-Critical Indexes

| Table | Column(s) | Purpose |
|-------|-----------|---------|
| questions | question_bank_id | Foreign key lookups |
| questions | category | Filtering by category |
| questions | difficulty | Difficulty filtering |
| tests | question_bank_id | Test queries by bank |
| tests | template_id | Template usage |
| test_questions | test_id | Get questions for test |
| answer_options | question_id | Get answer options |
| question_banks | owner_id | Owner's question banks |

## Constraints

### Not Null Constraints

- `questions.text` - Question text required
- `questions.category` - Category must be specified
- `questions.difficulty` - Difficulty level is required
- `answer_options.text` - Answer text required

### Check Constraints

- `questions.difficulty` BETWEEN 1 AND 3
- `tests.status` IN ('Draft', 'Active', 'Archived')
- `answer_options.is_correct` = TRUE/FALSE

### Foreign Keys

- `questions.question_bank_id` → `question_banks.id`
- `answer_options.question_id` → `questions.id`
- `test_questions.test_id` → `tests.id`
- `test_questions.question_id` → `questions.id`

## Sequences

```sql
-- Not used - UUIDs are used instead
```

## Views

### v_questions_with_bank

```sql
CREATE VIEW v_questions_with_bank AS
SELECT 
    q.id,
    q.text,
    q.category,
    q.difficulty,
    qb.name AS question_bank_name,
    qb.owner_id,
    q.created_at
FROM questions q
JOIN question_banks qb ON q.question_bank_id = qb.id
WHERE q.is_deleted = false;
```

## Triggers

### trg_soft_delete_question

```sql
CREATE OR REPLACE FUNCTION软 delete_question()
RETURNS TRIGGER AS $$
BEGIN
    NEW.is_deleted := TRUE;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_soft_delete_question
    BEFORE DELETE ON questions
    FOR EACH ROW
    EXECUTE FUNCTION soft_delete_question();
```

## Database Pooling

```csharp
// In Program.cs
options.UseNpgsql(connectionString, npgsqlOptions =>
{
    npgsqlOptions.Pooling = true;
    npgsqlOptions.MaxPoolSize = 20;
    npgsqlOptions.MinPoolSize = 5;
    npgsqlOptions.ConnectionIdleTimeout = 30;
});
```
