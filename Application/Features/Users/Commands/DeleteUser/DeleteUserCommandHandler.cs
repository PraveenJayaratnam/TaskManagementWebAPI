using Application.Common;
using Application.Services;

namespace Application.Features.Users.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly IUserService _userService;

        public DeleteUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var success = await _userService.DeleteAsync(request.Id);
                
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
