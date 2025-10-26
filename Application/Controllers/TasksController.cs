using Application.DTOs;
using Application.Features.Tasks.Commands;
using Application.Features.Tasks.Queries;

namespace Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController(IMediator mediator, ILogger<TasksController> logger) : ControllerBase
{
    [HttpGet("all")]
    public async Task<IActionResult> GetAllTasks([FromQuery] TaskFilterDto? filterDto = null)
    {
        try
        {
            if (filterDto == null)
            {
                var query = new GetAllTasksQuery();
                var result = await mediator.Send(query);
                
                if (result.IsSuccess)
                {
                    return Ok(result.Value);
                }
                
                return BadRequest(new { message = result.Error });
            }
            else
            {
                logger.LogInformation("Getting tasks for user with filters: {@FilterDto}", filterDto);
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                filterDto.UserId = userId;
                
                var query = new GetFilteredTasksQuery(filterDto);
                var result = await mediator.Send(query);
                
                if (result.IsSuccess)
                {
                    logger.LogInformation("Retrieved {TaskCount} tasks for user {UserId} (Page {PageIndex} of {TotalPages})", 
                        result.Value!.Items.Count, userId, result.Value.PageIndex, result.Value.TotalPages);
                    return Ok(result.Value);
                }
                
                return BadRequest(new { message = result.Error });
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting tasks");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("paginated")]
    public async Task<IActionResult> GetTasksPaginated([FromQuery] TaskFilterDto filterDto)
    {
        try
        {
            logger.LogInformation("Getting paginated tasks for user with filters: {@FilterDto}", filterDto);
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            filterDto.UserId = userId;
            
            var query = new GetFilteredTasksQuery(filterDto);
            var result = await mediator.Send(query);
            
            if (result.IsSuccess)
            {
                logger.LogInformation("Retrieved {TaskCount} tasks for user {UserId} (Page {PageIndex} of {TotalPages})", 
                    result.Value!.Items.Count, userId, result.Value.PageIndex, result.Value.TotalPages);
                return Ok(result.Value);
            }
            
            return BadRequest(new { message = result.Error });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while getting paginated tasks");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(Guid id)
    {
        try
        {
            var query = new GetTaskByIdQuery(id);
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

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTaskDto)
    {
        try
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var command = new CreateTaskCommand(userId, createTaskDto);
            var result = await mediator.Send(command);
            
            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(GetTask), new { id = result.Value!.Id }, result.Value);
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskDto updateTaskDto)
    {
        try
        {
            var command = new UpdateTaskCommand(id, updateTaskDto);
            var result = await mediator.Send(command);
            
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        try
        {
            var command = new DeleteTaskCommand(id);
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
}

