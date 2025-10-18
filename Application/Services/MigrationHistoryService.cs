using Infrastructure.Data;
using Domain.Entities;

namespace Application.Services;

public class MigrationHistoryService : IMigrationHistoryService
{
    private readonly ApplicationDbContext _context;
    private readonly IAuditService _auditService;

    public MigrationHistoryService(ApplicationDbContext context, IAuditService auditService)
    {
        _context = context;
        _auditService = auditService;
    }

    public async Task<MigrationHistory> RecordMigrationAsync(string migrationName, string version, 
        string? description = null, string? rollbackScript = null, string? notes = null)
    {
        var migration = new MigrationHistory
        {
            MigrationName = migrationName,
            Version = version,
            Description = description,
            AppliedAt = DateTime.UtcNow,
            AppliedBy = Environment.UserName ?? "System",
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
            RollbackScript = rollbackScript,
            Notes = notes,
            IsRolledBack = false
        };

        _context.MigrationHistory.Add(migration);
        await _context.SaveChangesAsync();
        return migration;
    }

    public async Task<IEnumerable<MigrationHistory>> GetAllMigrationsAsync()
    {
        return await _context.MigrationHistory
            .Where(m => !m.IsDeleted)
            .OrderByDescending(m => m.AppliedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<MigrationHistory>> GetMigrationsByEnvironmentAsync(string environment)
    {
        return await _context.MigrationHistory
            .Where(m => !m.IsDeleted && m.Environment == environment)
            .OrderByDescending(m => m.AppliedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<MigrationHistory>> GetMigrationsByVersionAsync(string version)
    {
        return await _context.MigrationHistory
            .Where(m => !m.IsDeleted && m.Version == version)
            .OrderByDescending(m => m.AppliedAt)
            .ToListAsync();
    }

    public async Task<MigrationHistory?> GetLatestMigrationAsync()
    {
        return await _context.MigrationHistory
            .Where(m => !m.IsDeleted)
            .OrderByDescending(m => m.AppliedAt)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<MigrationHistory>> GetMigrationsByDateRangeAsync(DateTime fromDate, DateTime toDate)
    {
        return await _context.MigrationHistory
            .Where(m => !m.IsDeleted && m.AppliedAt >= fromDate && m.AppliedAt <= toDate)
            .OrderByDescending(m => m.AppliedAt)
            .ToListAsync();
    }

    public async Task<bool> IsMigrationAppliedAsync(string migrationName, string version)
    {
        return await _context.MigrationHistory
            .AnyAsync(m => !m.IsDeleted && m.MigrationName == migrationName && m.Version == version);
    }

    public async Task<MigrationHistory> RecordRollbackAsync(Guid migrationId, string? notes = null)
    {
        var migration = await _context.MigrationHistory
            .FirstOrDefaultAsync(m => m.Id == migrationId && !m.IsDeleted);

        if (migration == null)
        {
            throw new KeyNotFoundException($"Migration with ID {migrationId} not found");
        }

        migration.IsRolledBack = true;
        migration.RolledBackAt = DateTime.UtcNow;
        migration.RolledBackBy = Environment.UserName ?? "System";
        migration.Notes = notes;

        _context.MigrationHistory.Update(migration);
        await _context.SaveChangesAsync();
        return migration;
    }

    public async Task<MigrationStatistics> GetMigrationStatisticsAsync()
    {
        var migrations = await _context.MigrationHistory
            .Where(m => !m.IsDeleted)
            .ToListAsync();

        var statistics = new MigrationStatistics
        {
            TotalMigrations = migrations.Count,
            AppliedMigrations = migrations.Count(m => !m.IsRolledBack),
            RolledBackMigrations = migrations.Count(m => m.IsRolledBack),
            LastMigrationDate = migrations.Max(m => m.AppliedAt),
            LastMigrationName = migrations.OrderByDescending(m => m.AppliedAt).FirstOrDefault()?.MigrationName
        };

        statistics.MigrationsByEnvironment = migrations
            .GroupBy(m => m.Environment)
            .ToDictionary(g => g.Key!, g => g.Count());

        statistics.MigrationsByVersion = migrations
            .GroupBy(m => m.Version)
            .ToDictionary(g => g.Key!, g => g.Count());

        return statistics;
    }

    public async Task<IEnumerable<string>> GetPendingMigrationsAsync()
    {
        return await Task.FromResult(Enumerable.Empty<string>());
    }

    public async Task<IEnumerable<MigrationHistory>> GetAppliedMigrationsAsync()
    {
        return await _context.MigrationHistory
            .Where(m => !m.IsDeleted && !m.IsRolledBack)
            .OrderByDescending(m => m.AppliedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<MigrationHistory>> GetRolledBackMigrationsAsync()
    {
        return await _context.MigrationHistory
            .Where(m => !m.IsDeleted && m.IsRolledBack)
            .OrderByDescending(m => m.RolledBackAt)
            .ToListAsync();
    }
}

