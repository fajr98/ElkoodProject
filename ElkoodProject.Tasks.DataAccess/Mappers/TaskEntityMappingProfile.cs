namespace ElkoodProject.Tasks.DataAccess.Mappers;

using AutoMapper;
using ElkoodProject.Tasks.DataAccess.Entities;
using ElkoodProject.Domain.Tasks.Models;

public class TaskEntityMappingProfile : Profile
{
    public TaskEntityMappingProfile()
    {
        CreateMap<TaskEntity, Task>();

        CreateMap<Task, TaskEntity>();

    }
}
