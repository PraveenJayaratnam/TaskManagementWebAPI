using Domain.Entities;

namespace Application.Services;

public interface IMigrationHistoryService
{
    Task<MigrationHistory> RecordMigrationAsync(string migrationName, string version, 
        string? description = null, string? rollbackScript = null, string? notes = null);

    Task<IEnumerable<MigrationHistory>> GetAllMigrationsAsync();

    Task<IEnumerable<MigrationHistory>> GetMigrationsByEnvironmentAsync(string environment);

    Task<IEnumerable<MigrationHistory>> GetMigrationsByVersionAsync(string version);

    Task<MigrationHistory?> GetLatestMigrationAsync();

    Task<IEnumerable<MigrationHistory>> GetMigrationsByDateRangeAsync(DateTime fromDate, DateTime toDate);

    Task<bool> IsMigrationAppliedAsync(string migrationName, string version);

    Task<MigrationHistory> RecordRollbackAsync(Guid migrationId, string? notes = null);

    Task<MigrationStatistics> GetMigrationStatisticsAsync();

    Task<IEnumerable<string>> GetPendingMigrationsAsync();

    Task<IEnumerable<MigrationHistory>> GetAppliedMigrationsAsync();

    Task<IEnumerable<MigrationHistory>> GetRolledBackMigrationsAsync();
}

public class MigrationStatistics
{
    public int TotalMigrations { get; set; }
    public int AppliedMigrations { get; set; }
    public int RolledBackMigrations { get; set; }
    public int PendingMigrations { get; set; }
    public DateTime? LastMigrationDate { get; set; }
    public string? LastMigrationName { get; set; }
    public Dictionary<string, int> MigrationsByEnvironment { get; set; } = new();
    public Dictionary<string, int> MigrationsByVersion { get; set; } = new();
}

