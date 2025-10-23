using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Users.Queries;

public class GetAllUsersQueryHandler(IUserService userService) : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserDto>>>
{

    public async Task<Result<IEnumerable<UserDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var users = await userService.GetAllAsync();
            return Result<IEnumerable<UserDto>>.Success(users);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<UserDto>>.Failure($"Get all users failed: {ex.Message}");
        }
    }
}

