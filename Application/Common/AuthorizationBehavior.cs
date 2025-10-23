namespace Application.Common;

public class AuthorizationBehavior<TRequest, TResponse>(
    IHttpContextAccessor httpContextAccessor) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var httpContext = httpContextAccessor.HttpContext;
        
        if (httpContext == null)
        {
            throw new UnauthorizedAccessException("HTTP context is not available");
        }

        var endpoint = httpContext.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null)
        {
            return await next();
        }

        var user = httpContext.User;
        
        if (user == null || !user.Identity?.IsAuthenticated == true)
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User ID claim not found");
        }

        var userId = userIdClaim.Value;
        
        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User ID is empty");
        }

        return await next();
    }
}
