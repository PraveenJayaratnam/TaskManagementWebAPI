using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Users.Queries;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserDto>>>
{
    private readonly IUserService _userService;

    public GetAllUsersQueryHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var users = await _userService.GetAllAsync();
            return Result<IEnumerable<UserDto>>.Success(users);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<UserDto>>.Failure($"Get all users failed: {ex.Message}");
        }
    }
}

