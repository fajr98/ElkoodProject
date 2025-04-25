namespace ElkoodProject.Domain.Tasks.Repositories;

using System;
using System.Collections.Generic;
using ElkoodProject.Domain.Tasks.Models;

public interface ITasksRepository
{
    Task<Task> AddAsync(Task task);

    Task<IEnumerable<Task>> GetAllAsync();

    Task<IEnumerable<Task>> GetAllAsync(TaskFilter taskFilter, int pageIndex, int pageSize);

    Task<Task> GetByIdAsync(Guid id);

    Task<Task> UpdateAsync(Guid id, Task task);

    System.Threading.Tasks.Task RemoveAsync(Guid id);
}
