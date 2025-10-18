using Application.DTOs;

namespace Application.Services;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(Guid id);
    Task<UserDto?> GetByUsernameAsync(string username);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> CreateAsync(CreateUserDto createUserDto);
    Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ValidateUserAsync(string username, string password);
    Task<bool> UsernameExistsAsync(string username);
}

