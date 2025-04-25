namespace ElkoodProject.Task.Application.Guest.Mappers;

using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ElkoodProject.Application.Contracts.Task.Guest.Dtos;
using ElkoodProject.Domain.Tasks.Models;

public class TaskDtoGuestMappingProfile : Profile
{
    public TaskDtoGuestMappingProfile()
    {
        CreateMap<Task, TaskGuestDto>();

        CreateMap<IEnumerable<Task>, TaskListItemsGuestDto>()
            .ForMember(dest => dest.Items, src => src.MapFrom(i => i.ToList()));

        CreateMap<TaskFilterDto, TaskFilter>();
    }
}
