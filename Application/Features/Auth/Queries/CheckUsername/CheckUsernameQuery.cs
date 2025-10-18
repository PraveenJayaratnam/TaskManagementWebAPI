using Application.Common;

namespace Application.Features.Auth.Queries;

public record CheckUsernameQuery(string Username) : IRequest<Result<bool>>;

