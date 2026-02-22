using Microsoft.EntityFrameworkCore;
using TestCheckingService.Domain.Entities;

namespace TestCheckingService.Infrastructure.Database.Context;

public class TestCheckingDbContext : DbContext, ITestCheckingDbContext
{
    public TestCheckingDbContext(DbContextOptions<TestCheckingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Test> Tests { get; set; } = null!;

    public virtual DbSet<TestResult> TestResults { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Test>(entity =>
        {
            entity.ToTable("tests");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Title)
                .HasColumnName("title")
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(e => e.PassPercentage)
                .HasColumnName("pass_percentage")
                .HasDefaultValue(70)
                .IsRequired();

            entity.Property(e => e.MaxScore)
                .HasColumnName("max_score")
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.HasIndex(e => e.Title).HasDatabaseName("ix_tests_title");
        });

        modelBuilder.Entity<TestResult>(entity =>
        {
            entity.ToTable("test_results");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.TestId)
                .HasColumnName("test_id")
                .IsRequired();

            entity.Property(e => e.StudentId)
                .HasColumnName("student_id")
                .IsRequired();

            entity.Property(e => e.Score)
                .HasColumnName("score")
                .IsRequired();

            entity.Property(e => e.MaxScore)
                .HasColumnName("max_score")
                .IsRequired();

            entity.Property(e => e.Percentage)
                .HasColumnName("percentage")
                .HasColumnType("DECIMAL(5,2)")
                .IsRequired();

            entity.Property(e => e.IsPassed)
                .HasColumnName("is_passed")
                .IsRequired();

            entity.Property(e => e.AttemptNumber)
                .HasColumnName("attempt_number")
                .HasDefaultValue(1)
                .IsRequired();

            entity.Property(e => e.PassedDate)
                .HasColumnName("passed_date");

            entity.Property(e => e.Answers)
                .HasColumnName("answers")
                .HasColumnType("JSONB")
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.HasIndex(e => e.TestId).HasDatabaseName("ix_test_results_test_id");
            entity.HasIndex(e => e.StudentId).HasDatabaseName("ix_test_results_student_id");
            entity.HasIndex(e => e.CreatedAt).HasDatabaseName("ix_test_results_created_at");

            entity.HasCheckConstraint(
                "ck_test_results_percentage_range",
                "percentage >= 0 AND percentage <= 100");
        });
    }
}
