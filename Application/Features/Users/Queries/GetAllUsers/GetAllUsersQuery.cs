using Application.Common;
using Application.DTOs;

namespace Application.Features.Users.Queries;

public record GetAllUsersQuery() : IRequest<Result<IEnumerable<UserDto>>>;

