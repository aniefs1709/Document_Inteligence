using DocumentIntelligence.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DocumentIntelligence.Identity.Infrastructure.Persistence;

public sealed class IdentityDbContext(
    DbContextOptions<IdentityDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var user = modelBuilder.Entity<User>();

        user.ToTable("Users", "identity");

        user.HasKey(x => x.Id);

        user.Property(x => x.Id)
            .ValueGeneratedNever();

        user.Property(x => x.Email)
            .HasMaxLength(320)
            .IsRequired();

        user.Property(x => x.NormalizedEmail)
            .HasMaxLength(320)
            .IsRequired();

        user.HasIndex(x => x.NormalizedEmail)
            .IsUnique();

        user.Property(x => x.PasswordHash)
            .HasMaxLength(512)
            .IsRequired();

        user.Property(x => x.DisplayName)
            .HasMaxLength(100)
            .IsRequired();

        user.Property(x => x.IsActive)
            .IsRequired();

        user.Property(x => x.CreatedAt)
            .IsRequired();
    }
}
