using Application.Common;
using Infrastructure.UnitOfWork;
using MediatR;

namespace Application.Features.Auth.Queries;

public class CheckUsernameQueryHandler : IRequestHandler<CheckUsernameQuery, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckUsernameQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(CheckUsernameQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var usernameExists = await _unitOfWork.Users.UsernameExistsAsync(request.Username);
            return Result<bool>.Success(usernameExists); // Return true if username exists, false if available
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Error checking username availability: {ex.Message}");
        }
    }
}
