using Application.Common;
using Application.Services;

namespace Application.Features.Users.Commands;

public class ActivateUserCommandHandler(IUserService userService) : IRequestHandler<ActivateUserCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await userService.ActivateAsync(request.Id);
            return Result<bool>.Success(result);
        }
        catch (KeyNotFoundException ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Activate user failed: {ex.Message}");
        }
    }
}
