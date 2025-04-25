namespace ElkoodProject.Task.Application.Owner.Mappers;

using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ElkoodProject.Application.Contracts.Task.Owner.Dtos;
using ElkoodProject.Domain.Tasks.Models;

public class TaskDtoOwnerMappingProfile : Profile
{
    public TaskDtoOwnerMappingProfile()
    {
        CreateMap<Task, TaskDto>();

        CreateMap<IEnumerable<Task>, TaskListItemsDto>()
            .ForMember(dest => dest.Items, src => src.MapFrom(i => i.ToList()));

        CreateMap<TaskFilterDto, TaskFilter>();

        CreateMap<AddTaskDto, Task>();

        CreateMap<UpdateTaskDto, Task>();
    }
}
