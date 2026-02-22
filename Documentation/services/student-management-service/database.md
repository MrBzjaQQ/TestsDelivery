# Student Management Service - Database Schema

## Tables

### students

```sql
CREATE TABLE students (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    user_id UUID NOT NULL REFERENCES identity_users(id),
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    email VARCHAR(255) NOT NULL,
    group_id UUID REFERENCES study_groups(id),
    phone_number VARCHAR(20),
    profile JSONB,
    status VARCHAR(20) NOT NULL DEFAULT 'Active',
    enrollment_date TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now()),
    created_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now())
);

CREATE INDEX IX_students_user_id ON students(user_id);
CREATE INDEX IX_students_group_id ON students(group_id);
CREATE INDEX IX_students_status ON students(status);
```

### study_groups

```sql
CREATE TABLE study_groups (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(100) NOT NULL,
    description TEXT,
    start_year SMALLINT NOT NULL,
    end_year SMALLINT NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now())
);
```

### test_assignments

```sql
CREATE TABLE test_assignments (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    student_id UUID NOT NULL REFERENCES students(id) ON DELETE CASCADE,
    test_id UUID NOT NULL REFERENCES tests(id) ON DELETE CASCADE,
    assigned_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now()),
    deadline TIMESTAMP WITH TIME ZONE,
    status VARCHAR(20) NOT NULL DEFAULT 'Assigned',
    attempts_allowed SMALLINT NOT NULL DEFAULT 3,
    attempts_used SMALLINT NOT NULL DEFAULT 0,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now())
);

CREATE INDEX IX_test_assignments_student_id ON test_assignments(student_id);
CREATE INDEX IX_test_assignments_test_id ON test_assignments(test_id);
CREATE INDEX IX_test_assignments_status ON test_assignments(status);
```

### test_progress

```sql
CREATE TABLE test_progress (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    student_id UUID NOT NULL REFERENCES students(id) ON DELETE CASCADE,
    test_id UUID NOT NULL REFERENCES tests(id) ON DELETE CASCADE,
    attempt_number SMALLINT NOT NULL,
    score SMALLINT,
    max_score SMALLINT NOT NULL,
    is_passed BOOLEAN NOT NULL,
    submitted_at TIMESTAMP WITH TIME ZONE,
    answers JSONB,
    created_at TIMESTAMP WITH TIME ZONE DEFAULT timezone('utc'::text, now()),
    UNIQUE(student_id, test_id, attempt_number)
);

CREATE INDEX IX_test_progressStudent_id ON test_progress(student_id);
CREATE INDEX IX_test_progress_test_id ON test_progress(test_id);
```

## Migrations

```bash
dotnet ef migrations add Create_StudentTables \
  --project StudentManagementService.Infrastructure.Database \
  --startup-project ../StudentManagementService.WebApi
```
