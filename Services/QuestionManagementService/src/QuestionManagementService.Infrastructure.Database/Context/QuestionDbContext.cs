using Microsoft.EntityFrameworkCore;
using QuestionManagementService.Domain.Entities;

namespace QuestionManagementService.Infrastructure.Database.Context;

public class QuestionDbContext : DbContext, IQuestionDbContext
{
    public QuestionDbContext(DbContextOptions<QuestionDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Question> Questions { get; set; } = null!;

    public virtual DbSet<QuestionBank> QuestionBanks { get; set; } = null!;

    public virtual DbSet<Test> Tests { get; set; } = null!;

    public virtual DbSet<TestTemplate> TestTemplates { get; set; } = null!;

    public virtual DbSet<AnswerOption> AnswerOptions { get; set; } = null!;

    public virtual DbSet<TestQuestion> TestQuestions { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<QuestionBank>(entity =>
        {
            entity.ToTable("question_banks");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").HasColumnType("TEXT");
            entity.Property(e => e.OwnerId).HasColumnName("owner_id").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasIndex(e => e.OwnerId).HasDatabaseName("ix_question_banks_owner_id");
            entity.HasIndex(e => e.Name).HasDatabaseName("ix_question_banks_name");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("questions");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Text).HasColumnName("text").HasColumnType("TEXT").IsRequired();
            entity.Property(e => e.Category).HasColumnName("category").HasMaxLength(50).IsRequired();
            entity.Property(e => e.Difficulty).HasColumnName("difficulty").IsRequired();
            entity.Property(e => e.QuestionBankId).HasColumnName("question_bank_id").IsRequired();
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasOne(e => e.QuestionBank)
                .WithMany(e => e.Questions)
                .HasForeignKey(e => e.QuestionBankId)
                .HasConstraintName("fk_questions_question_banks");

            entity.HasMany(e => e.AnswerOptions)
                .WithOne(e => e.Question)
                .HasForeignKey(e => e.QuestionId)
                .HasConstraintName("fk_answer_options_questions");

            entity.HasIndex(e => e.QuestionBankId).HasDatabaseName("ix_questions_question_bank_id");
            entity.HasIndex(e => e.Category).HasDatabaseName("ix_questions_category");
            entity.HasIndex(e => e.Difficulty).HasDatabaseName("ix_questions_difficulty");
        });

        modelBuilder.Entity<AnswerOption>(entity =>
        {
            entity.ToTable("answer_options");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.QuestionId).HasColumnName("question_id").IsRequired();
            entity.Property(e => e.Text).HasColumnName("text").HasMaxLength(500).IsRequired();
            entity.Property(e => e.IsCorrect).HasColumnName("is_correct").HasDefaultValue(false);
            entity.Property(e => e.Ordinal).HasColumnName("ordinal").IsRequired();

            entity.HasIndex(e => e.QuestionId).HasDatabaseName("ix_answer_options_question_id");
            entity.HasIndex(e => e.Ordinal).HasDatabaseName("ix_answer_options_ordinal");
        });

        modelBuilder.Entity<TestTemplate>(entity =>
        {
            entity.ToTable("test_templates");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Name).HasColumnName("name").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").HasColumnType("TEXT");
            entity.Property(e => e.DefaultDuration).HasColumnName("default_duration").IsRequired();
            entity.Property(e => e.DefaultPassingScore).HasColumnName("default_passing_score").IsRequired();
            entity.Property(e => e.Configuration).HasColumnName("configuration").HasColumnType("JSONB");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasIndex(e => e.Name).HasDatabaseName("ix_test_templates_name");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.ToTable("tests");

            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id").HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Title).HasColumnName("title").HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasColumnName("description").HasColumnType("TEXT");
            entity.Property(e => e.QuestionBankId).HasColumnName("question_bank_id").IsRequired();
            entity.Property(e => e.TemplateId).HasColumnName("template_id");
            entity.Property(e => e.DurationMinutes).HasColumnName("duration_minutes").IsRequired();
            entity.Property(e => e.PassingScore).HasColumnName("passing_score").IsRequired();
            entity.Property(e => e.MaxAttempts).HasColumnName("max_attempts").IsRequired();
            entity.Property(e => e.Status).HasColumnName("status").HasMaxLength(20).HasDefaultValue("Draft");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at").IsRequired();
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted").HasDefaultValue(false);

            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasOne(e => e.QuestionBank)
                .WithMany()
                .HasForeignKey(e => e.QuestionBankId)
                .HasConstraintName("fk_tests_question_banks");

            entity.HasOne(e => e.Template)
                .WithMany()
                .HasForeignKey(e => e.TemplateId)
                .HasConstraintName("fk_tests_test_templates");

            entity.HasIndex(e => e.QuestionBankId).HasDatabaseName("ix_tests_question_bank_id");
            entity.HasIndex(e => e.TemplateId).HasDatabaseName("ix_tests_template_id");
            entity.HasIndex(e => e.Status).HasDatabaseName("ix_tests_status");
        });

        modelBuilder.Entity<TestQuestion>(entity =>
        {
            entity.ToTable("test_questions");

            entity.HasKey(e => new { e.TestId, e.QuestionId });

            entity.Property(e => e.TestId).HasColumnName("test_id");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.Ordinal).HasColumnName("ordinal").IsRequired();

            entity.HasOne(e => e.Test)
                .WithMany(e => e.TestQuestions)
                .HasForeignKey(e => e.TestId)
                .HasConstraintName("fk_test_questions_tests");

            entity.HasOne(e => e.Question)
                .WithMany()
                .HasForeignKey(e => e.QuestionId)
                .HasConstraintName("fk_test_questions_questions");
        });
    }
}
