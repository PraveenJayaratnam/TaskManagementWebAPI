using Application.Common;
using Application.Services;

namespace Application.Features.Users.Commands
{
    public class DeleteUserCommandHandler(IUserService userService) : IRequestHandler<DeleteUserCommand, Result>
    {

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var success = await userService.DeleteAsync(request.Id);
                
                if (success)
                {
                    return Result.Success();
                }
                
                return Result.Failure("User not found");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Delete user failed: {ex.Message}");
            }
        }
    }
}
