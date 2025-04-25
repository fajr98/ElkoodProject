namespace ElkoodProject.Tasks.DataAccess.Repositories;

using System;
using System.Collections.Generic;
using AutoMapper;
using ElkoodProject.Domain.Tasks.Models;
using ElkoodProject.Domain.Tasks.Repositories;
using ElkoodProject.Task.DataAccess;
using ElkoodProject.Tasks.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

public class TasksRepository : ITasksRepository
{
    private readonly TasksDbContext _context;
    private readonly IMapper _mapper;

    public TasksRepository(TasksDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Task> AddAsync(Task task)
    {
        ArgumentNullException.ThrowIfNull(task, nameof(task));

        var entity = _mapper.Map<TaskEntity>(task);

        var result = await _context.Tasks.AddAsync(entity);
        await _context.SaveChangesAsync();

        return _mapper.Map<Task>(result.Entity);
    }

    public async Task<IEnumerable<Task>> GetAllAsync()
    {
        return _mapper.Map<IEnumerable<Task>>(await _context.Tasks.AsNoTracking().ToListAsync());
    }

    public async Task<IEnumerable<Task>> GetAllAsync(TaskFilter taskFilter, int pageIndex, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(taskFilter);
        var query = BuildQuery(taskFilter).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        return _mapper.Map<IEnumerable<Task>>(await query.AsNoTracking().ToListAsync());
    }

    public async Task<Task> GetByIdAsync(Guid id)
    {
        return _mapper.Map<Task>(await _context.Tasks.FindAsync(id));
    }

    public async System.Threading.Tasks.Task RemoveAsync(Guid id)
    {
        var entity = await _context.Tasks.FindAsync(id);

        _context.Entry(entity!).State = EntityState.Deleted;

        await _context.SaveChangesAsync();
    }

    public async Task<Task> UpdateAsync(Guid id, Task task)
    {
        ArgumentNullException.ThrowIfNull(task, nameof(task));

        var entity = await _context.Tasks.FindAsync(id);

        entity!.Name = !string.IsNullOrEmpty(task.Name) ? task.Name : entity.Name;
        entity!.Description = !string.IsNullOrEmpty(task.Description) ? task.Description : entity.Description;
        entity!.DiedLineInHours = task.DiedLineInHours != 0 ? task.DiedLineInHours : entity.DiedLineInHours;
        entity!.Status = Enum.IsDefined(typeof(TaskStatus), task.Status) ? task.Status : entity.Status;
        entity!.Category = Enum.IsDefined(typeof(TaskCategory), task.Category) ? task.Category : entity.Category;
        entity!.Priority = task.Priority != 0 ? task.Priority : entity.Priority;

        _context.Entry(entity).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return _mapper.Map<Task>(entity);
    }

    private IQueryable<TaskEntity> BuildQuery(TaskFilter filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        var query = _context.Tasks.AsQueryable();

        if (filter.Name != null && !string.IsNullOrEmpty(filter.Name))
        {
            query = query.Where(a => a.Name.Contains(filter.Name));
        }
        if (filter.DiedLineInHours != null)
        {
            query = query.Where(a => a.DiedLineInHours.Equals(filter.DiedLineInHours));
        }
        if (filter.Status != null)
        {
            query = query.Where(a => a.Status.Equals(filter.Status));
        }
        if (filter.Category != null)
        {
            query = query.Where(a => a.Category.Equals(filter.Category));
        }
        if (filter.Priority != null)
        {
            query = query.Where(a => a.Priority.Equals(filter.Priority));
        }

        return query;
    }
}
