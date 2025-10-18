using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text.Json;
using Application.Services;

namespace Application.Authentication;

public class CustomAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IUserService _userService;

    public CustomAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IUserService userService)
        : base(options, logger, encoder)
    {
        _userService = userService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-User", out var userHeader))
        {
            return AuthenticateResult.Fail("X-User header is missing");
        }

        var userHeaderValue = userHeader.FirstOrDefault();
        if (string.IsNullOrEmpty(userHeaderValue))
        {
            return AuthenticateResult.Fail("X-User header value is empty");
        }

        try
        {
            // Try to parse as JSON first (new format)
            UserInfo? userInfo = null;
            try
            {
                userInfo = JsonSerializer.Deserialize<UserInfo>(userHeaderValue);
            }
            catch (JsonException)
            {
                // If JSON parsing fails, treat as plain user ID (legacy format)
                var userId = userHeaderValue;
                var legacyUser = await _userService.GetByIdAsync(Guid.Parse(userId));
                if (legacyUser == null)
                {
                    return AuthenticateResult.Fail("User not found");
                }

                var legacyClaims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, legacyUser.Id.ToString()),
                    new Claim(ClaimTypes.Name, legacyUser.Username)
                };

                var legacyIdentity = new ClaimsIdentity(legacyClaims, Scheme.Name);
                var legacyPrincipal = new ClaimsPrincipal(legacyIdentity);
                var legacyTicket = new AuthenticationTicket(legacyPrincipal, Scheme.Name);

                return AuthenticateResult.Success(legacyTicket);
            }

            // If JSON parsing succeeded, use the user info directly
            if (userInfo == null || string.IsNullOrEmpty(userInfo.Id))
            {
                return AuthenticateResult.Fail("Invalid user information in X-User header");
            }

            // Validate that the user exists in the database
            var user = await _userService.GetByIdAsync(Guid.Parse(userInfo.Id));
            if (user == null)
            {
                return AuthenticateResult.Fail("User not found");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userInfo.Id),
                new Claim(ClaimTypes.Name, userInfo.Username ?? user.Username)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error during authentication");
            return AuthenticateResult.Fail("Authentication failed");
        }
    }

    private class UserInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
