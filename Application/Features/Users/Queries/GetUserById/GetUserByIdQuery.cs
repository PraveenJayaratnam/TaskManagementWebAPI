using Application.Common;
using Application.DTOs;

namespace Application.Features.Users.Queries;

public record GetUserByIdQuery(Guid Id) : IRequest<Result<UserDto>>;

