using Application.Common;
using Application.DTOs;
using Application.Services;

namespace Application.Features.Tasks.Commands;

public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, Result<TaskDto>>
{
    private readonly ITaskService _taskService;

    public CreateTaskCommandHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<Result<TaskDto>> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var taskDto = await _taskService.CreateAsync(request.UserId, request.CreateTaskDto);
            return Result<TaskDto>.Success(taskDto);
        }
        catch (Exception ex)
        {
            return Result<TaskDto>.Failure($"Create task failed: {ex.Message}");
        }
    }
}
