using Application.DTOs;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Application.Services;

public class AuthService(IGenericRepository<User> userRepository) : IAuthService
{

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        var user = await userRepository.FirstOrDefaultAsync(u => u.Username == loginRequest.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException("Your account has been deactivated. Please contact support.");
        }

        var userDto = user.Adapt<UserDto>();
        return new LoginResponse 
        { 
            User = userDto, 
            Success = true, 
            Message = "Login successful" 
        };
    }

    public async Task<UserDto> RegisterAsync(RegisterRequest registerRequest)
    {
        if (await userRepository.ExistsAsync(u => u.Username == registerRequest.Username))
        {
            throw new InvalidOperationException("Username already exists");
        }

        var user = registerRequest.Adapt<User>();
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
        
        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();
        
        return user.Adapt<UserDto>();
    }


    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id);
        return user != null ? user.Adapt<UserDto>() : null;
    }

    public async Task<bool> CheckUsernameExistsAsync(string username)
    {
        return await userRepository.ExistsAsync(u => u.Username == username);
    }
}

