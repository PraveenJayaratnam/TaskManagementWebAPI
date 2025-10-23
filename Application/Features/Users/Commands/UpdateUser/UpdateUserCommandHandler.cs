using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Users.Commands;

public class UpdateUserCommandHandler(IUserService userService) : IRequestHandler<UpdateUserCommand, Result<UserDto>>
{

    public async Task<Result<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userDto = await userService.UpdateAsync(request.Id, request.UpdateUserDto);
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

