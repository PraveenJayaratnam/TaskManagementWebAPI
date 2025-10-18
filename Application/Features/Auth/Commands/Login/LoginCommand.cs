using Application.Common;
using Application.DTOs;

namespace Application.Features.Auth.Commands;

public record LoginCommand(string Username, string Password) : IRequest<Result<LoginResponse>>;

