using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Users.Commands;

public class CreateUserCommandHandler(IUserService userService) : IRequestHandler<CreateUserCommand, Result<UserDto>>
{

    public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userDto = await userService.CreateAsync(request.CreateUserDto);
            return Result<UserDto>.Success(userDto);
        }
        catch (InvalidOperationException ex)
        {
            return Result<UserDto>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure($"Create user failed: {ex.Message}");
        }
    }
}

