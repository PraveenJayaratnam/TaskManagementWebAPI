using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Users.Commands;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserDto>>
{
    private readonly IUserService _userService;

    public UpdateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var updateUserDto = new UpdateUserDto
            {
                Username = request.Username,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var userDto = await _userService.UpdateAsync(request.Id, updateUserDto);
            return Result<UserDto>.Success(userDto);
        }
        catch (KeyNotFoundException ex)
        {
            return Result<UserDto>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure($"Update user failed: {ex.Message}");
        }
    }
}

