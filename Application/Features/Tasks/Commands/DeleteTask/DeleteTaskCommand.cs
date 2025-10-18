using Application.Common;

namespace Application.Features.Tasks.Commands;

public record DeleteTaskCommand(Guid Id) : IRequest<Result>;
