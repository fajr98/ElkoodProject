namespace ElkoodProject.Application.Contracts.Task.Guest.Dtos;

using ElkoodProject.Domain.Tasks.Models;

public class TaskFilterDto
{
    public string Name { get; set; } = default!;

    public int DiedLineInHours { get; set; }

    public TaskStatus Status { get; set; }

    public TaskCategory Category { get; set; }

    public int Priority { get; set; }
}
