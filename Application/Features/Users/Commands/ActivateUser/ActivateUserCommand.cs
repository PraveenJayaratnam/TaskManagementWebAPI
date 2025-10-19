using Application.Common;

namespace Application.Features.Users.Commands;

public record ActivateUserCommand(Guid Id) : IRequest<Result<bool>>;
