namespace ElkoodProject.Task.Application.Guest.Services;

using System;
using AutoMapper;
using ElkoodProject.Application.Contracts.Task.Guest.Contracts;
using ElkoodProject.Application.Contracts.Task.Guest.Dtos;
using ElkoodProject.Domain.Tasks.Models;
using ElkoodProject.Domain.Tasks.Repositories;

public class TasksGuestService : ITasksGuestService
{
    private readonly ITasksRepository _tasksRepository;
    private readonly IMapper _mapper;

    public TasksGuestService(ITasksRepository tasksRepository, IMapper mapper)
    {
        _tasksRepository = tasksRepository;
        _mapper = mapper;
    }

    public async Task<TaskListItemsGuestDto> GetAllAsync(TaskFilterDto taskFilterDto, int pageIndex, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(taskFilterDto, nameof(taskFilterDto));

        var filter = _mapper.Map<TaskFilter>(taskFilterDto);

        return _mapper.Map<TaskListItemsGuestDto>(await _tasksRepository.GetAllAsync(filter, pageIndex, pageSize));
    }
}
