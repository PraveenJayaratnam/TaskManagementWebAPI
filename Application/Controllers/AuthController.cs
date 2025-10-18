using Application.DTOs;
using Application.Features.Auth.Commands;
using Application.Features.Auth.Queries;
using Application.Services;

namespace Application.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;
    private readonly IAuditService _auditService;

    public AuthController(IMediator mediator, ILogger<AuthController> logger, IAuditService auditService)
    {
        _mediator = mediator;
        _logger = logger;
        _auditService = auditService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        try
        {
            var command = new LoginCommand(loginRequest.Username, loginRequest.Password);
            var result = await _mediator.Send(command);
            
            if (result.IsSuccess)
            {
                if (result.Value?.User?.Id != null && Guid.TryParse(result.Value.User.Id, out var userId))
                {
                    _auditService.SetCurrentUserId(userId);
                }
                
                return Ok(result.Value);
            }
            
            return Unauthorized(new LoginResponse { Success = false, Message = result.Error });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new LoginResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new LoginResponse { Success = false, Message = ex.Message });
        }
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        try
        {
            var command = new RegisterCommand(
                registerRequest.Username, 
                registerRequest.Password, 
                registerRequest.FirstName, 
                registerRequest.LastName);
            
            var result = await _mediator.Send(command);
            
            if (result.IsSuccess)
            {
                if (result.Value?.Id != null && Guid.TryParse(result.Value.Id, out var userId))
                {
                    _auditService.SetCurrentUserId(userId);
                }
                
                return CreatedAtAction("GetUser", new { id = result.Value!.Id }, result.Value);
            }
            
            return Conflict(new { message = result.Error });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("check-username")]
    [AllowAnonymous]
    public async Task<IActionResult> CheckUsername([FromQuery] string username)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return BadRequest(new { message = "Username is required" });
            }

            var query = new CheckUsernameQuery(username);
            var result = await _mediator.Send(query);
            
            if (result.IsSuccess)
            {
                return Ok(new { exists = result.Value, username });
            }
            
            return BadRequest(new { message = result.Error });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}

