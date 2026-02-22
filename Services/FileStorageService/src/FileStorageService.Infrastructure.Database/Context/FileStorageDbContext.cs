using Microsoft.EntityFrameworkCore;
using DomainEntities = FileStorageService.Domain.Entities;

namespace FileStorageService.Infrastructure.Database.Context;

public class FileStorageDbContext : DbContext, IFileStorageDbContext
{
    public FileStorageDbContext(DbContextOptions<FileStorageDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DomainEntities.File> Files { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DomainEntities.File>(entity =>
        {
            entity.ToTable("files");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.FileName)
                .HasColumnName("file_name")
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(e => e.ContentType)
                .HasColumnName("content_type")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Size)
                .HasColumnName("size")
                .IsRequired();

            entity.Property(e => e.Data)
                .HasColumnName("data")
                .HasColumnType("bytea")
                .IsRequired();

            entity.Property(e => e.OwnerId)
                .HasColumnName("owner_id")
                .IsRequired();

            entity.Property(e => e.Width)
                .HasColumnName("width");

            entity.Property(e => e.Height)
                .HasColumnName("height");

            entity.Property(e => e.IsImage)
                .HasColumnName("is_image")
                .HasDefaultValue(false);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.HasIndex(e => e.OwnerId).HasDatabaseName("ix_files_owner_id");
            entity.HasIndex(e => e.CreatedAt).HasDatabaseName("ix_files_created_at");
        });
    }
}
