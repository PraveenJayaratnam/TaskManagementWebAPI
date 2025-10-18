using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data
{
    public static class DbSets
    {
        public static void ConfigureDbSets(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Username).IsUnique();
            });

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Status).HasConversion<int>();
                entity.Property(e => e.Priority).HasConversion<int>();
                
                entity.HasOne(e => e.User)
                    .WithMany(e => e.Tasks)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<MigrationHistory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MigrationName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Version).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.AppliedAt).IsRequired();
                entity.Property(e => e.AppliedBy).HasMaxLength(100);
                entity.Property(e => e.Environment).HasMaxLength(50);
                entity.Property(e => e.RollbackScript).HasMaxLength(1000);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.IsRolledBack).HasDefaultValue(false);
                entity.Property(e => e.RolledBackBy).HasMaxLength(100);
                
                entity.HasIndex(e => new { e.MigrationName, e.Version }).IsUnique();
                
                entity.HasIndex(e => e.AppliedAt);
                
                entity.HasIndex(e => e.Environment);
            });
        }

        public static void SeedData(ModelBuilder modelBuilder)
        {
            var adminUserId = Guid.NewGuid();
            var currentTime = DateTime.UtcNow;

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = adminUserId,
                    Username = "admin",
                    FirstName = "Admin",
                    LastName = "User",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    CreatedAt = currentTime,
                    IsActive = true,
                    IsDeleted = false
                }
            );
        }
    }
}

