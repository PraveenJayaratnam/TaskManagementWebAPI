using Application.Common;
using Application.Services;

namespace Application.Features.Users.Commands;

public class ActivateUserCommandHandler : IRequestHandler<ActivateUserCommand, Result<bool>>
{
    private readonly IUserService _userService;

    public ActivateUserCommandHandler(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Result<bool>> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _userService.ActivateAsync(request.Id);
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
