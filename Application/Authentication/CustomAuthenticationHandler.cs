using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Services;

namespace Application.Authentication;

public class CustomAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    IUserService userService) 
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, UrlEncoder.Default)
{

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
            Logger.LogInformation("X-User header value: {UserHeaderValue}", userHeaderValue);
            
            UserInfo? userInfo = null;
            try
            {
                var jsonDoc = JsonDocument.Parse(userHeaderValue);
                var root = jsonDoc.RootElement;
                
                userInfo = new UserInfo
                {
                    Id = root.GetProperty("id").GetString() ?? string.Empty,
                    Username = root.GetProperty("username").GetString() ?? string.Empty,
                    FirstName = root.GetProperty("firstName").GetString() ?? string.Empty,
                    LastName = root.GetProperty("lastName").GetString() ?? string.Empty
                };
            }
            catch (JsonException ex)
            {
                Logger.LogInformation("JSON parsing failed, Error: {Error}", ex.Message);
                
                var userId = userHeaderValue;
                var legacyUser = await userService.GetByIdAsync(Guid.Parse(userId));
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

            if (userInfo == null || string.IsNullOrEmpty(userInfo.Id))
            {
                Logger.LogWarning("Invalid user information in X-User header. UserInfo is null: {IsNull}, UserId: {UserId}", userInfo == null, userInfo?.Id);
                return AuthenticateResult.Fail("Invalid user information in X-User header");
            }

            var user = await userService.GetByIdAsync(Guid.Parse(userInfo.Id));
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
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        
        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;
        
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } = string.Empty;
        
        [JsonPropertyName("lastName")]
        public string LastName { get; set; } = string.Empty;
    }
}
