using Application.DTOs;

namespace Application.Services;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest loginRequest);
    Task<UserDto> RegisterAsync(RegisterRequest registerRequest);
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<bool> CheckUsernameExistsAsync(string username);
}

