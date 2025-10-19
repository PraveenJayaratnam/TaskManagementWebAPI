using Application.Common;

namespace Application.Features.Users.Commands;

public record DeactivateUserCommand(Guid Id) : IRequest<Result<bool>>;
