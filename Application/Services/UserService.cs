using Application.DTOs;
using Domain.Entities;
using Infrastructure.Repositories;

namespace Application.Services;

public class UserService(IGenericRepository<User> userRepository) : IUserService
{

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id);
        return user != null ? user.Adapt<UserDto>() : null;
    }

    public async Task<UserDto?> GetByUsernameAsync(string username)
    {
        var user = await userRepository.FirstOrDefaultAsync(u => u.Username == username);
        return user != null ? user.Adapt<UserDto>() : null;
    }


    public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
    {
        if (await userRepository.ExistsAsync(u => u.Username == createUserDto.Username))
        {
            throw new InvalidOperationException("Username already exists");
        }

        var user = createUserDto.Adapt<User>();
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
        
        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();
        
        return user.Adapt<UserDto>();
    }

    public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        updateUserDto.Adapt(user);
        await userRepository.UpdateAsync(user);
        await userRepository.SaveChangesAsync();
        
        return user.Adapt<UserDto>();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null) return false;

        await userRepository.SoftDeleteAsync(user);
        await userRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeactivateAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null) return false;

        user.IsActive = false;
        await userRepository.UpdateAsync(user);
        await userRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ActivateAsync(Guid id)
    {
        var user = await userRepository.GetByIdAsync(id);
        if (user == null) return false;

        user.IsActive = true;
        await userRepository.UpdateAsync(user);
        await userRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ValidateUserAsync(string username, string password)
    {
        var user = await userRepository.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null || !user.IsActive) return false;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await userRepository.ExistsAsync(u => u.Username == username);
    }

    public async Task<IQueryable<UserDto>> GetAllAsync()
    {
        var query = await userRepository.GetAllQueryable();
        return query.Select(u => u.Adapt<UserDto>());
    }
}

