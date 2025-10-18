using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Auth.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<UserDto>>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Result<UserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var registerRequest = new RegisterRequest
            {
                Username = request.Username,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var userDto = await _authService.RegisterAsync(registerRequest);
            return Result<UserDto>.Success(userDto);
        }
        catch (InvalidOperationException ex)
        {
            return Result<UserDto>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure($"Registration failed: {ex.Message}");
        }
    }
}

