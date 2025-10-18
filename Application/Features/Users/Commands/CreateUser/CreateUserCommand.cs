using Application.Common;
using Application.DTOs;

namespace Application.Features.Users.Commands;

public record CreateUserCommand(string Username, string Password, string? FirstName, string? LastName) 
    : IRequest<Result<UserDto>>;

