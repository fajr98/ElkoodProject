namespace ElkoodProject.Task.Application.Owner.Services;

using System;
using AutoMapper;
using ElkoodProject.Application.Contracts.Task.Owner.Contracts;
using ElkoodProject.Application.Contracts.Task.Owner.Dtos;
using ElkoodProject.Domain.Tasks.Models;
using ElkoodProject.Domain.Tasks.Repositories;

public class TasksOwnerService : ITasksOwnerService
{
    private readonly ITasksRepository _tasksRepository;
    private readonly IMapper _mapper;

    public TasksOwnerService(ITasksRepository tasksRepository, IMapper mapper)
    {
        _tasksRepository = tasksRepository;
        _mapper = mapper;
    }

    public async Task<TaskDto> AddAsync(AddTaskDto addTaskDto)
    {
        ArgumentNullException.ThrowIfNull(addTaskDto, nameof(addTaskDto));

        var task = _mapper.Map<Task>(addTaskDto);

        var result = await _tasksRepository.AddAsync(task);

        return _mapper.Map<TaskDto>(result);
    }

    public async Task<TaskListItemsDto> GetAllAsync(TaskFilterDto taskFilterDto, int pageIndex, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(taskFilterDto, nameof(taskFilterDto));

        var filter = _mapper.Map<TaskFilter>(taskFilterDto);

        return _mapper.Map<TaskListItemsDto>(await _tasksRepository.GetAllAsync(filter, pageIndex, pageSize));
    }

    public async System.Threading.Tasks.Task RemoveAsync(Guid id)
    {
        await _tasksRepository.RemoveAsync(id);
    }

    public async Task<TaskDto> UpdateAsync(Guid id, UpdateTaskDto updateTaskDto)
    {
        ArgumentNullException.ThrowIfNull(updateTaskDto, nameof(updateTaskDto));

        var task = _mapper.Map<Task>(updateTaskDto);

        var result = await _tasksRepository.UpdateAsync(id, task);

        return _mapper.Map<TaskDto>(result);
    }
}
