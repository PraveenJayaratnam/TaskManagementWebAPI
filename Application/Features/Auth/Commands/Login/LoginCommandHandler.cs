using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Auth.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var loginRequest = new LoginRequest
            {
                Username = request.Username,
                Password = request.Password
            };

            var response = await _authService.LoginAsync(loginRequest);
            return Result<LoginResponse>.Success(response);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Result<LoginResponse>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return Result<LoginResponse>.Failure($"Login failed: {ex.Message}");
        }
    }
}

