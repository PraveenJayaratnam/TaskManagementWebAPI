using Application.DTOs;
using Application.Features.Tasks.Commands;
using Application.Features.Tasks.Queries;
using Domain.Enums;
using System.Security.Claims;

namespace Application.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<TasksController> _logger;

    public TasksController(IMediator mediator, ILogger<TasksController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks([FromQuery] TaskFilterDto? filterDto = null)
    {
        try
        {
            if (filterDto == null)
            {
                var query = new GetAllTasksQuery();
                var result = await _mediator.Send(query);
                
                if (result.IsSuccess)
                {
                    return Ok(result.Value);
                }
                
                return BadRequest(new { message = result.Error });
            }
            else
            {
                _logger.LogInformation("Getting tasks for user with filters: {@FilterDto}", filterDto);
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                filterDto.UserId = userId;
                
                var query = new GetFilteredTasksQuery(filterDto);
                var result = await _mediator.Send(query);
                
                if (result.IsSuccess)
                {
                    _logger.LogInformation("Retrieved {TaskCount} tasks for user {UserId} (Page {PageIndex} of {TotalPages})", 
                        result.Value!.Items.Count, userId, result.Value.PageIndex, result.Value.TotalPages);
                    return Ok(result.Value);
                }
                
                return BadRequest(new { message = result.Error });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting tasks");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("paginated")]
    public async Task<IActionResult> GetTasksPaginated([FromQuery] TaskFilterDto filterDto)
    {
        try
        {
            _logger.LogInformation("Getting paginated tasks for user with filters: {@FilterDto}", filterDto);
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            filterDto.UserId = userId;
            
            var query = new GetFilteredTasksQuery(filterDto);
            var result = await _mediator.Send(query);
            
            if (result.IsSuccess)
            {
                _logger.LogInformation("Retrieved {TaskCount} tasks for user {UserId} (Page {PageIndex} of {TotalPages})", 
                    result.Value!.Items.Count, userId, result.Value.PageIndex, result.Value.TotalPages);
                return Ok(result.Value);
            }
            
            return BadRequest(new { message = result.Error });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting paginated tasks");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTask(Guid id)
    {
        try
        {
            var query = new GetTaskByIdQuery(id);
            var result = await _mediator.Send(query);
            
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
            var result = await _mediator.Send(command);
            
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
            var result = await _mediator.Send(command);
            
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
            var result = await _mediator.Send(command);
            
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

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetTasksByUser(Guid userId)
    {
        try
        {
            var currentUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (userId != currentUserId)
                return Forbid("You can only view your own tasks");

            var query = new GetTasksByUserQuery(userId);
            var result = await _mediator.Send(query);
            
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

}

