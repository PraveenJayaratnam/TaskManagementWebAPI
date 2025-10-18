using Application.DTOs;
using Application.Features.Users.Queries;
using Application.Features.Users.Commands;

namespace Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IMediator mediator, ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);
            
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            
            return BadRequest(new { message = result.Error });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (id != userId)
                return Forbid("You can only view your own profile");

            var query = new GetUserByIdQuery(id);
            var result = await _mediator.Send(query);
            
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            
            return NotFound(new { message = result.Error });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (id != userId)
                return Forbid("You can only update your own profile");

            var command = new UpdateUserCommand(id, updateUserDto.Username, updateUserDto.FirstName, updateUserDto.LastName);
            var result = await _mediator.Send(command);
            
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            
            return BadRequest(new { message = result.Error });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (id != userId)
                return Forbid("You can only delete your own profile");

            var command = new DeleteUserCommand(id);
            var result = await _mediator.Send(command);
            
            if (result.IsSuccess)
            {
                return NoContent();
            }
            
            return NotFound(new { message = result.Error });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
        {
            throw new UnauthorizedAccessException("Invalid user token");
        }
        return userId;
    }
}

