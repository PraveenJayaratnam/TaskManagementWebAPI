using Application.Common;
using Application.DTOs;

namespace Application.Features.Auth.Commands;

public record RegisterCommand(
    string Username,
    string Password,
    string FirstName,
    string LastName
) : IRequest<Result<UserDto>>;
