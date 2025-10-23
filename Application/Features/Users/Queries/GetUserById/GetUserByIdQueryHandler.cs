using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Users.Queries;

public class GetUserByIdQueryHandler(IUserService userService) : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
{

    public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userService.GetByIdAsync(request.Id);
            if (user == null)
            {
                return Result<UserDto>.Failure("User not found");
            }

            return Result<UserDto>.Success(user);
        }
        catch (Exception ex)
        {
            return Result<UserDto>.Failure($"Get user failed: {ex.Message}");
        }
    }
}
