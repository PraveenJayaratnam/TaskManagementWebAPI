using Application.Common;
using Application.Features.Tasks.Commands;
using Application.Services;

namespace Application.Features.Tasks.Commands;

public class DeleteTaskCommandHandler(ITaskService taskService) : IRequestHandler<DeleteTaskCommand, Result>
{

    public async Task<Result> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var success = await taskService.DeleteAsync(request.Id);
            
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
