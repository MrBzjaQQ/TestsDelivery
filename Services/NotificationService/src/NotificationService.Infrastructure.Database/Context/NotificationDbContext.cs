using NotificationService.Domain.Entities;
using NotificationService.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace NotificationService.Infrastructure.Database.Context;

public class NotificationDbContext : DbContext, INotificationDbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Notification> Notifications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.ToTable("notifications");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.To)
                .HasColumnName("to")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.Subject)
                .HasColumnName("subject")
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(e => e.TemplateName)
                .HasColumnName("template_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.TemplateData)
                .HasColumnName("template_data")
                .HasColumnType("jsonb");

            entity.Property(e => e.Type)
                .HasColumnName("type")
                .HasConversion(
                    v => (int)v,
                    v => (NotificationType)v)
                .IsRequired();

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasConversion(
                    v => (int)v,
                    v => (NotificationStatus)v)
                .IsRequired();

            entity.Property(e => e.RetryCount)
                .HasColumnName("retry_count")
                .HasDefaultValue(0);

            entity.Property(e => e.MaxRetries)
                .HasColumnName("max_retries")
                .HasDefaultValue(3);

            entity.Property(e => e.ErrorMessage)
                .HasColumnName("error_message")
                .HasMaxLength(2000);

            entity.Property(e => e.HtmlBody)
                .HasColumnName("html_body")
                .HasColumnType("text");

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(e => e.SentAt)
                .HasColumnName("sent_at");

            entity.Property(e => e.RelatedEntityId)
                .HasColumnName("related_entity_id");

            entity.Property(e => e.RelatedEntityType)
                .HasColumnName("related_entity_type")
                .HasMaxLength(50);

            entity.HasIndex(e => e.Status).HasDatabaseName("ix_notifications_status");
            entity.HasIndex(e => e.CreatedAt).HasDatabaseName("ix_notifications_created_at");
            entity.HasIndex(e => e.To).HasDatabaseName("ix_notifications_to");
        });
    }
}
