using Application.Common;
using Application.DTOs;

namespace Application.Features.Users.Commands;

public record UpdateUserCommand(Guid Id, UpdateUserDto UpdateUserDto) 
    : IRequest<Result<UserDto>>;

