using Application.Common;
using Application.DTOs;

namespace Application.Features.Users.Commands;

public record UpdateUserCommand(Guid Id, string Username, string? FirstName, string? LastName) 
    : IRequest<Result<UserDto>>;

