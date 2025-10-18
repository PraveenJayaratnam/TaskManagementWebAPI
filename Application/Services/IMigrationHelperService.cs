namespace Application.Services;

public interface IMigrationHelperService
{
    Task RecordInitialMigrationAsync();

    Task RecordSchemaMigrationAsync(string migrationName, string description, string? rollbackScript = null);

    Task RecordDataMigrationAsync(string migrationName, string description, string? rollbackScript = null);

    Task RecordSeedMigrationAsync(string migrationName, string description);

    Task RecordCleanupMigrationAsync(string migrationName, string description);

    Task<string> GetCurrentDatabaseVersionAsync();

    Task<bool> IsDatabaseUpToDateAsync();

    Task<MigrationSummary> GetMigrationSummaryAsync();
}

public class MigrationSummary
{
    public string CurrentVersion { get; set; } = string.Empty;
    public int TotalMigrations { get; set; }
    public int AppliedMigrations { get; set; }
    public int RolledBackMigrations { get; set; }
    public DateTime? LastMigrationDate { get; set; }
    public string? LastMigrationName { get; set; }
    public List<string> RecentMigrations { get; set; } = new();
    public bool IsUpToDate { get; set; }
}

