namespace ElkoodProject.Application.Contracts.Task.Owner.Dtos;

using ElkoodProject.Domain.Tasks.Models;

public class AddTaskDto
{
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public int DiedLineInHours { get; set; }

    public TaskStatus Status { get; set; }

    public TaskCategory Category { get; set; }

    public int Priority { get; set; }
}
