using Application.Common;
using Application.Services;

namespace Application.Features.Users.Commands;

public class DeactivateUserCommandHandler : IRequestHandler<DeactivateUserCommand, Result<bool>>
{
    private readonly IUserService _userService;

    public DeactivateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<bool>> Handle(DeactivateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _userService.DeactivateAsync(request.Id);
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
