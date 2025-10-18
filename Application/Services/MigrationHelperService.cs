namespace Application.Services;

public class MigrationHelperService : IMigrationHelperService
{
    private readonly IMigrationHistoryService _migrationHistoryService;
    private readonly ILogger<MigrationHelperService> _logger;

    public MigrationHelperService(
        IMigrationHistoryService migrationHistoryService,
        ILogger<MigrationHelperService> logger)
    {
        _migrationHistoryService = migrationHistoryService;
        _logger = logger;
    }

    public async Task RecordInitialMigrationAsync()
    {
        try
        {
            var migrationName = "InitialDatabaseCreation";
            var version = "1.0.0";
            var description = "Initial database creation with User and TaskItem tables";

            var exists = await _migrationHistoryService.IsMigrationAppliedAsync(migrationName, version);
            if (!exists)
            {
                await _migrationHistoryService.RecordMigrationAsync(
                    migrationName,
                    version,
                    description,
                    null,
                    "Automatically recorded during database initialization");

                _logger.LogInformation("Recorded initial database creation migration");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording initial migration");
            throw;
        }
    }

    public async Task RecordSchemaMigrationAsync(string migrationName, string description, string? rollbackScript = null)
    {
        try
        {
            var version = await GetNextVersionAsync();
            await _migrationHistoryService.RecordMigrationAsync(
                migrationName,
                version,
                description,
                rollbackScript,
                "Schema change migration");

            _logger.LogInformation("Recorded schema migration: {MigrationName} version {Version}", migrationName, version);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording schema migration: {MigrationName}", migrationName);
            throw;
        }
    }

    public async Task RecordDataMigrationAsync(string migrationName, string description, string? rollbackScript = null)
    {
        try
        {
            var version = await GetNextVersionAsync();
            await _migrationHistoryService.RecordMigrationAsync(
                migrationName,
                version,
                description,
                rollbackScript,
                "Data migration");

            _logger.LogInformation("Recorded data migration: {MigrationName} version {Version}", migrationName, version);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording data migration: {MigrationName}", migrationName);
            throw;
        }
    }

    public async Task RecordSeedMigrationAsync(string migrationName, string description)
    {
        try
        {
            var version = await GetNextVersionAsync();
            await _migrationHistoryService.RecordMigrationAsync(
                migrationName,
                version,
                description,
                null,
                "Seed data migration");

            _logger.LogInformation("Recorded seed migration: {MigrationName} version {Version}", migrationName, version);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording seed migration: {MigrationName}", migrationName);
            throw;
        }
    }

    public async Task RecordCleanupMigrationAsync(string migrationName, string description)
    {
        try
        {
            var version = await GetNextVersionAsync();
            await _migrationHistoryService.RecordMigrationAsync(
                migrationName,
                version,
                description,
                null,
                "Cleanup migration");

            _logger.LogInformation("Recorded cleanup migration: {MigrationName} version {Version}", migrationName, version);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error recording cleanup migration: {MigrationName}", migrationName);
            throw;
        }
    }

    public async Task<string> GetCurrentDatabaseVersionAsync()
    {
        try
        {
            var latestMigration = await _migrationHistoryService.GetLatestMigrationAsync();
            return latestMigration?.Version ?? "0.0.0";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current database version");
            return "0.0.0";
        }
    }

    public async Task<bool> IsDatabaseUpToDateAsync()
    {
        try
        {
            var currentVersion = await GetCurrentDatabaseVersionAsync();
            return currentVersion != "0.0.0";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking if database is up to date");
            return false;
        }
    }

    public async Task<MigrationSummary> GetMigrationSummaryAsync()
    {
        try
        {
            var statistics = await _migrationHistoryService.GetMigrationStatisticsAsync();
            var recentMigrations = await _migrationHistoryService.GetAllMigrationsAsync();
            var isUpToDate = await IsDatabaseUpToDateAsync();

            return new MigrationSummary
            {
                CurrentVersion = statistics.LastMigrationName ?? "0.0.0",
                TotalMigrations = statistics.TotalMigrations,
                AppliedMigrations = statistics.AppliedMigrations,
                RolledBackMigrations = statistics.RolledBackMigrations,
                LastMigrationDate = statistics.LastMigrationDate,
                LastMigrationName = statistics.LastMigrationName,
                RecentMigrations = recentMigrations.Take(5).Select(m => $"{m.MigrationName} v{m.Version}").ToList(),
                IsUpToDate = isUpToDate
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting migration summary");
            return new MigrationSummary
            {
                CurrentVersion = "0.0.0",
                IsUpToDate = false
            };
        }
    }

    private async Task<string> GetNextVersionAsync()
    {
        try
        {
            var latestMigration = await _migrationHistoryService.GetLatestMigrationAsync();
            if (latestMigration == null)
            {
                return "1.0.0";
            }

            var versionParts = latestMigration.Version.Split('.');
            if (versionParts.Length == 3 && int.TryParse(versionParts[2], out var patch))
            {
                return $"{versionParts[0]}.{versionParts[1]}.{patch + 1}";
            }

            return "1.0.0";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting next version");
            return "1.0.0";
        }
    }
}

