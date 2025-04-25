namespace ElkoodProject.Application.Contracts.Task.Owner.Contracts;

using ElkoodProject.Application.Contracts.Task.Owner.Dtos;

public interface ITasksOwnerService
{
    Task<TaskListItemsDto> GetAllAsync(TaskFilterDto taskFilterDto, int pageIndex, int pageSize);

    Task<TaskDto> AddAsync(AddTaskDto addTaskDto);

    Task<TaskDto> UpdateAsync(Guid id, UpdateTaskDto updateTaskDto);

    System.Threading.Tasks.Task RemoveAsync(Guid id);
}
