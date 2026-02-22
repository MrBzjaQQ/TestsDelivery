using IdentityService.Infrastructure.Database.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Database.Context;

public class IdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IIdentityDbContext
{
    public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(entity =>
        {
            entity.ToTable("users");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserName).HasColumnName("user_name");
            entity.Property(e => e.NormalizedUserName).HasColumnName("normalized_user_name");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.NormalizedEmail).HasColumnName("normalized_email");
            entity.Property(e => e.EmailConfirmed).HasColumnName("email_confirmed");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.SecurityStamp).HasColumnName("security_stamp");
            entity.Property(e => e.ConcurrencyStamp).HasColumnName("concurrency_stamp");
            entity.Property(e => e.PhoneNumber).HasColumnName("phone_number");
            entity.Property(e => e.PhoneNumberConfirmed).HasColumnName("phone_number_confirmed");
            entity.Property(e => e.TwoFactorEnabled).HasColumnName("two_factor_enabled");
            entity.Property(e => e.LockoutEnd).HasColumnName("lockout_end");
            entity.Property(e => e.LockoutEnabled).HasColumnName("lockout_enabled");
            entity.Property(e => e.AccessFailedCount).HasColumnName("access_failed_count");

            entity.Property(e => e.FirstName)
                .HasColumnName("first_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.LastName)
                .HasColumnName("last_name")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Role)
                .HasColumnName("role")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(e => e.EmailVerified)
                .HasColumnName("email_verified")
                .HasDefaultValue(false);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.HasIndex(e => e.Email).HasDatabaseName("ix_users_email");
            entity.HasIndex(e => e.CreatedAt).HasDatabaseName("ix_users_created_at");
        });

        builder.Entity<ApplicationRole>(entity =>
        {
            entity.ToTable("roles");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.NormalizedName).HasColumnName("normalized_name");
            entity.Property(e => e.ConcurrencyStamp).HasColumnName("concurrency_stamp");
        });

        builder.Entity<IdentityUserRole<string>>(entity =>
        {
            entity.ToTable("user_roles");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
        });

        builder.Entity<IdentityUserClaim<string>>(entity =>
        {
            entity.ToTable("user_claims");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.ClaimType).HasColumnName("claim_type");
            entity.Property(e => e.ClaimValue).HasColumnName("claim_value");
        });

        builder.Entity<IdentityUserLogin<string>>(entity =>
        {
            entity.ToTable("user_logins");

            entity.Property(e => e.LoginProvider).HasColumnName("login_provider");
            entity.Property(e => e.ProviderKey).HasColumnName("provider_key");
            entity.Property(e => e.ProviderDisplayName).HasColumnName("provider_display_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");
        });

        builder.Entity<IdentityRoleClaim<string>>(entity =>
        {
            entity.ToTable("role_claims");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.ClaimType).HasColumnName("claim_type");
            entity.Property(e => e.ClaimValue).HasColumnName("claim_value");
        });

        builder.Entity<IdentityUserToken<string>>(entity =>
        {
            entity.ToTable("user_tokens");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.LoginProvider).HasColumnName("login_provider");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Value).HasColumnName("value");
        });

        builder.Entity<RefreshToken>(entity =>
        {
            entity.ToTable("refresh_tokens");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Token).HasColumnName("token").IsRequired();
            entity.Property(e => e.UserId).HasColumnName("user_id").IsRequired();
            entity.Property(e => e.Created).HasColumnName("created").IsRequired();
            entity.Property(e => e.Expires).HasColumnName("expires").IsRequired();
            entity.Property(e => e.CreatedByIp).HasColumnName("created_by_ip");
            entity.Property(e => e.Revoked).HasColumnName("revoked");
            entity.Property(e => e.RevokedByIp).HasColumnName("revoked_by_ip");
            entity.Property(e => e.ReplacedByToken).HasColumnName("replaced_by_token");

            entity.HasOne(e => e.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.Token).HasDatabaseName("ix_refresh_tokens_token");
            entity.HasIndex(e => e.UserId).HasDatabaseName("ix_refresh_tokens_user_id");
        });
    }
}
