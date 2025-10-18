using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserDto>>
{
    private readonly IUserService _userService;

    public CreateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var createUserDto = new CreateUserDto
            {
                Username = request.Username,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            var userDto = await _userService.CreateAsync(createUserDto);
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

