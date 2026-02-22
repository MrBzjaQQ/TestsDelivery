using BffPortalService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BffPortalService.Infrastructure.Database.Context;

public class BffPortalDbContext : DbContext, IBffPortalDbContext
{
    public BffPortalDbContext(DbContextOptions<BffPortalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PortalCacheItem> PortalCacheItems { get; set; } = null!;

    public virtual DbSet<PortalSettings> PortalSettings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PortalCacheItem>(entity =>
        {
            entity.ToTable("portal_cache_items");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.CacheKey)
                .HasColumnName("cache_key")
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(e => e.CacheValue)
                .HasColumnName("cache_value")
                .HasColumnType("text")
                .IsRequired();

            entity.Property(e => e.CacheType)
                .HasColumnName("cache_type")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(e => e.ExpiresAt)
                .HasColumnName("expires_at")
                .IsRequired();

            entity.HasIndex(e => e.CacheKey)
                .IsUnique()
                .HasDatabaseName("ix_portal_cache_items_cache_key");

            entity.HasIndex(e => e.ExpiresAt)
                .HasDatabaseName("ix_portal_cache_items_expires_at");
        });

        modelBuilder.Entity<PortalSettings>(entity =>
        {
            entity.ToTable("portal_settings");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.SettingKey)
                .HasColumnName("setting_key")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.SettingValue)
                .HasColumnName("setting_value")
                .HasColumnType("text")
                .IsRequired();

            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(500);

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .IsRequired();

            entity.HasIndex(e => e.SettingKey)
                .IsUnique()
                .HasDatabaseName("ix_portal_settings_setting_key");
        });
    }
}
