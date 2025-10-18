namespace Application.Services;

public interface IAuditService
{
    void SetCurrentUserId(Guid userId);
    Guid? GetCurrentUserId();
    void LogAuditEvent(string action, string entityType, Guid entityId, string? details = null);
}