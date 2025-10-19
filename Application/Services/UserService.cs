using Application.DTOs;
using Domain.Entities;
using Infrastructure.UnitOfWork;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto?> GetByIdAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        return user != null ? user.Adapt<UserDto>() : null;
    }

    public async Task<UserDto?> GetByUsernameAsync(string username)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(username);
        return user != null ? user.Adapt<UserDto>() : null;
    }


    public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
    {
        if (await _unitOfWork.Users.UsernameExistsAsync(createUserDto.Username))
        {
            throw new InvalidOperationException("Username already exists");
        }

        var user = createUserDto.Adapt<User>();
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);
        
        _unitOfWork.Users.Add(user);
        await _unitOfWork.SaveChangesAsync();
        
        return user.Adapt<UserDto>();
    }

    public async Task<UserDto> UpdateAsync(Guid id, UpdateUserDto updateUserDto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        updateUserDto.Adapt(user);
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();
        
        return user.Adapt<UserDto>();
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null) return false;

        _unitOfWork.Users.SoftDelete(user);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeactivateAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null) return false;

        user.IsActive = false;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ActivateAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null) return false;

        user.IsActive = true;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ValidateUserAsync(string username, string password)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(username);
        if (user == null || !user.IsActive) return false;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _unitOfWork.Users.UsernameExistsAsync(username);
    }

    public async Task<IQueryable<UserDto>> GetAllAsync()
    {
        var query = await _unitOfWork.Users.GetAllQueryable();
        return await Task.FromResult(query.Select(u => u.Adapt<UserDto>()));
    }
}

