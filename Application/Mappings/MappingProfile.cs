using Application.DTOs;
using Domain.Entities;

namespace Application.Mappings;

public class MappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserDto>()
            .Map(dest => dest.Id, src => src.Id.ToString())
            .Map(dest => dest.Username, src => src.Username)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.IsActive, src => src.IsActive);

        config.NewConfig<CreateUserDto, User>()
            .Ignore(dest => dest.PasswordHash)
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.IsDeleted)
            .Ignore(dest => dest.Tasks);

        config.NewConfig<UpdateUserDto, User>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.PasswordHash)
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
            .Map(dest => dest.DueDate, src => src.DueDate)
            .Map(dest => dest.Priority, src => src.Priority);

        config.NewConfig<CreateTaskDto, TaskItem>()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.Status)
            .Map(dest => dest.DueDate, src => src.DueDate)
            .Map(dest => dest.Priority, src => src.Priority)
            .Ignore(dest => dest.IsActive)
            .Ignore(dest => dest.IsDeleted)
            .Ignore(dest => dest.CompletedAt)
            .Ignore(dest => dest.UserId)
            .Ignore(dest => dest.User);

        config.NewConfig<UpdateTaskDto, TaskItem>()
            .Ignore(dest => dest.Id)
            .Map(dest => dest.DueDate, src => src.DueDate)
            .Map(dest => dest.Priority, src => src.Priority)
            .Ignore(dest => dest.IsActive)
            .Ignore(dest => dest.IsDeleted)
            .Ignore(dest => dest.CompletedAt)
            .Ignore(dest => dest.UserId)
            .Ignore(dest => dest.User);
    }
}

