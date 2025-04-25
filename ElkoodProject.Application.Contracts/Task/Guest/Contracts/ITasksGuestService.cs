namespace ElkoodProject.Application.Contracts.Task.Guest.Contracts;

using ElkoodProject.Application.Contracts.Task.Guest.Dtos;

public interface ITasksGuestService
{
    Task<TaskListItemsGuestDto> GetAllAsync(TaskFilterDto taskFilterDto, int pageIndex, int pageSize);
}
