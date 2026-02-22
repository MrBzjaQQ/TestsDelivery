using Microsoft.EntityFrameworkCore;
using StudentManagementService.Domain.Entities;

namespace StudentManagementService.Infrastructure.Database.Context;

public class StudentDbContext : DbContext, IStudentDbContext
{
    public StudentDbContext(DbContextOptions<StudentDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Student> Students { get; set; } = null!;

    public virtual DbSet<StudyGroup> StudyGroups { get; set; } = null!;

    public virtual DbSet<TestAssignment> TestAssignments { get; set; } = null!;

    public virtual DbSet<TestProgress> TestProgresses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudyGroup>(entity =>
        {
            entity.ToTable("study_groups");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasColumnType("text");

            entity.Property(e => e.StartYear)
                .HasColumnName("start_year")
                .IsRequired();

            entity.Property(e => e.EndYear)
                .HasColumnName("end_year")
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.ToTable("students");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            entity.Property(e => e.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Email)
                .HasColumnName("email")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.GroupId).HasColumnName("group_id");

            entity.Property(e => e.PhoneNumber)
                .HasColumnName("phone_number")
                .HasMaxLength(20);

            entity.Property(e => e.Profile)
                .HasColumnName("profile")
                .HasColumnType("jsonb");

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(20)
                .HasDefaultValue(StudentStatus.Active)
                .IsRequired();

            entity.Property(e => e.EnrollmentDate)
                .HasColumnName("enrollment_date")
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.HasIndex(e => e.UserId).HasDatabaseName("ix_students_user_id");
            entity.HasIndex(e => e.GroupId).HasDatabaseName("ix_students_group_id");
            entity.HasIndex(e => e.Status).HasDatabaseName("ix_students_status");

            entity.HasOne<StudyGroup>()
                .WithMany()
                .HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<TestAssignment>(entity =>
        {
            entity.ToTable("test_assignments");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.StudentId)
                .HasColumnName("student_id")
                .IsRequired();

            entity.Property(e => e.TestId)
                .HasColumnName("test_id")
                .IsRequired();

            entity.Property(e => e.AssignedAt)
                .HasColumnName("assigned_at")
                .IsRequired();

            entity.Property(e => e.Deadline).HasColumnName("deadline");

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(20)
                .HasDefaultValue(TestAssignmentStatus.Assigned)
                .IsRequired();

            entity.Property(e => e.AttemptsAllowed)
                .HasColumnName("attempts_allowed")
                .HasDefaultValue((short)3)
                .IsRequired();

            entity.Property(e => e.AttemptsUsed)
                .HasColumnName("attempts_used")
                .HasDefaultValue((short)0)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.HasIndex(e => e.StudentId).HasDatabaseName("ix_test_assignments_student_id");
            entity.HasIndex(e => e.TestId).HasDatabaseName("ix_test_assignments_test_id");
            entity.HasIndex(e => e.Status).HasDatabaseName("ix_test_assignments_status");

            entity.HasOne<Student>()
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TestProgress>(entity =>
        {
            entity.ToTable("test_progress");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");

            entity.Property(e => e.StudentId)
                .HasColumnName("student_id")
                .IsRequired();

            entity.Property(e => e.TestId)
                .HasColumnName("test_id")
                .IsRequired();

            entity.Property(e => e.AttemptNumber)
                .HasColumnName("attempt_number")
                .IsRequired();

            entity.Property(e => e.Score).HasColumnName("score");

            entity.Property(e => e.MaxScore)
                .HasColumnName("max_score")
                .IsRequired();

            entity.Property(e => e.IsPassed)
                .HasColumnName("is_passed")
                .IsRequired();

            entity.Property(e => e.SubmittedAt).HasColumnName("submitted_at");

            entity.Property(e => e.Answers)
                .HasColumnName("answers")
                .HasColumnType("jsonb");

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.HasIndex(e => e.StudentId).HasDatabaseName("ix_test_progress_student_id");
            entity.HasIndex(e => e.TestId).HasDatabaseName("ix_test_progress_test_id");

            entity.HasOne<Student>()
                .WithMany()
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasAlternateKey(e => new { e.StudentId, e.TestId, e.AttemptNumber });
        });
    }
}
