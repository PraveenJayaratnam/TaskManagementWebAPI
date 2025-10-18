using Application.Common;
using Application.DTOs;

namespace Application.Features.Users.Commands;

public record CreateUserCommand(CreateUserDto CreateUserDto) 
    : IRequest<Result<UserDto>>;

