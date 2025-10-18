using Application.DTOs;
using Domain.Entities;

namespace Application.Mappings;

public class MappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserDto>()
            .Map(dest => dest.Id, src => src.Id.ToString());

        config.NewConfig<CreateUserDto, User>()
            .Ignore(dest => dest.PasswordHash)
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.IsActive)
            .Ignore(dest => dest.IsDeleted)
            .Ignore(dest => dest.Tasks);

        config.NewConfig<UpdateUserDto, User>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.PasswordHash)
            .Ignore(dest => dest.IsActive)
            .Ignore(dest => dest.IsDeleted)
            .Ignore(dest => dest.Tasks);

        config.NewConfig<RegisterRequest, User>()
            .Ignore(dest => dest.PasswordHash)
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.IsActive)
            .Ignore(dest => dest.IsDeleted)
            .Ignore(dest => dest.Tasks);

        config.NewConfig<TaskItem, TaskDto>()
            .Map(dest => dest.Id, src => src.Id.ToString())
            .Map(dest => dest.DueDate, src => src.DueDate.HasValue ? src.DueDate.Value.ToString("yyyy-MM-dd") : null)
            .Map(dest => dest.UserName, src => $"{src.User.FirstName} {src.User.LastName}");

        config.NewConfig<CreateTaskDto, TaskItem>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Status)
            .Ignore(dest => dest.Priority)
            .Map(dest => dest.DueDate, src => !string.IsNullOrEmpty(src.DueDate) ? DateTime.Parse(src.DueDate) : (DateTime?)null)
            .Ignore(dest => dest.IsActive)
            .Ignore(dest => dest.IsDeleted)
            .Ignore(dest => dest.CompletedAt)
            .Ignore(dest => dest.UserId)
            .Ignore(dest => dest.User);

        config.NewConfig<UpdateTaskDto, TaskItem>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Priority)
            .Map(dest => dest.DueDate, src => !string.IsNullOrEmpty(src.DueDate) ? DateTime.Parse(src.DueDate) : (DateTime?)null)
            .Ignore(dest => dest.IsActive)
            .Ignore(dest => dest.IsDeleted)
            .Ignore(dest => dest.CompletedAt)
            .Ignore(dest => dest.UserId)
            .Ignore(dest => dest.User);
    }
}

