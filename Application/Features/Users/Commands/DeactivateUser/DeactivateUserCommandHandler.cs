using Application.Common;
using Application.Services;

namespace Application.Features.Users.Commands;

public class DeactivateUserCommandHandler(IUserService userService) : IRequestHandler<DeactivateUserCommand, Result<bool>>
{

    public async Task<Result<bool>> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await userService.DeactivateAsync(request.Id);
            return Result<bool>.Success(result);
        }
        catch (KeyNotFoundException ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Deactivate user failed: {ex.Message}");
        }
    }
}
