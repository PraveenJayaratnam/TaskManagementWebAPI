using Application.Common;
using Application.Features.Tasks.Commands;
using Application.Services;

namespace Application.Features.Tasks.Commands;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Result>
{
    private readonly ITaskService _taskService;

    public DeleteTaskCommandHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public async Task<Result> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await _taskService.DeleteAsync(request.Id);
            
            if (success)
            {
                return Result.Success();
            }
            
            return Result.Failure("Task not found");
        }
        catch (Exception ex)
        {
            return Result.Failure($"Delete task failed: {ex.Message}");
        }
    }
}
