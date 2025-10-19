using Application.DTOs;
using Domain.Entities;
using Infrastructure.UnitOfWork;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(loginRequest.Username);
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
        if (await _unitOfWork.Users.UsernameExistsAsync(registerRequest.Username))
        {
            throw new InvalidOperationException("Username already exists");
        }

        var user = registerRequest.Adapt<User>();
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password);
        
        _unitOfWork.Users.Add(user);
        await _unitOfWork.SaveChangesAsync();
        
        return user.Adapt<UserDto>();
    }


    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        return user != null ? user.Adapt<UserDto>() : null;
    }

    public async Task<bool> CheckUsernameExistsAsync(string username)
    {
        return await _unitOfWork.Users.UsernameExistsAsync(username);
    }
}

