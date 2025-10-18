using Application.Common;

namespace Application.Features.Users.Commands;

public record DeleteUserCommand(Guid Id) : IRequest<Result>;

