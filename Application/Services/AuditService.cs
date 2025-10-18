namespace Application.Services;

public class AuditService : IAuditService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Guid? _currentUserId;

    public AuditService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetCurrentUserId(Guid userId)
    {
        _currentUserId = userId;
    }

    public Guid? GetCurrentUserId()
    {
        return _currentUserId;
    }

    public void LogAuditEvent(string action, string entityType, Guid entityId, string? details = null)
    {
        var auditEntry = new
        {
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            UserId = _currentUserId,
            Timestamp = DateTime.UtcNow,
            Details = details
        };
        
        Console.WriteLine($"Audit: {System.Text.Json.JsonSerializer.Serialize(auditEntry)}");
    }
}