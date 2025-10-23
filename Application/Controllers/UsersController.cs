using Application.DTOs;
using Application.Features.Users.Queries;
using Application.Features.Users.Commands;

namespace Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var query = new GetAllUsersQuery();
            var result = await mediator.Send(query);
            
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            
            return BadRequest(new { message = result.Error });
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            if (id != userId)
                return BadRequest(new { message = "You can only view your own profile" });

            var query = new GetUserByIdQuery(id);
            var result = await mediator.Send(query);
            
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            
            return NotFound(new { message = result.Error });
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            if (id != userId)
                return BadRequest(new { message = "You can only update your own profile" });

            var command = new UpdateUserCommand(id, updateUserDto);
            var result = await mediator.Send(command);
            
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            
            return BadRequest(new { message = result.Error });
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            if (id != userId)
                return BadRequest(new { message = "You can only delete your own profile" });

            var command = new DeleteUserCommand(id);
            var result = await mediator.Send(command);
            
            if (result.IsSuccess)
            {
                return NoContent();
            }
            
            return NotFound(new { message = result.Error });
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

    [HttpPatch("{id}/activate")]
    public async Task<IActionResult> ActivateUser(Guid id)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            if (id != userId)
                return BadRequest(new { message = "You can only activate your own profile" });

            var command = new ActivateUserCommand(id);
            var result = await mediator.Send(command);
            
            if (result.IsSuccess)
            {
                return Ok(new { message = "User activated successfully" });
            }
            
            return BadRequest(new { message = result.Error });
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

    [HttpPatch("{id}/deactivate")]
    public async Task<IActionResult> DeactivateUser(Guid id)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            
            if (id != userId)
                return BadRequest(new { message = "You can only deactivate your own profile" });

            var command = new DeactivateUserCommand(id);
            var result = await mediator.Send(command);
            
            if (result.IsSuccess)
            {
                return Ok(new { message = "User deactivated successfully" });
            }
            
            return BadRequest(new { message = result.Error });
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

}

