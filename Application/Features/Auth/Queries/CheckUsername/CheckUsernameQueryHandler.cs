using Application.Common;
using Infrastructure.Repositories;
using MediatR;
using Domain.Entities;

namespace Application.Features.Auth.Queries;

public class CheckUsernameQueryHandler(IGenericRepository<User> userRepository) : IRequestHandler<CheckUsernameQuery, Result<bool>>
{

    public async Task<Result<bool>> Handle(CheckUsernameQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var usernameExists = await userRepository.ExistsAsync(u => u.Username == request.Username);
            return Result<bool>.Success(usernameExists);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Error checking username availability: {ex.Message}");
        }
    }
}
