namespace ElkoodProject.Application.Contracts.Task.Owner.Dtos;

using ElkoodProject.Domain.Tasks.Models;

public class UpdateTaskDto
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? DiedLineInHours { get; set; }

    public TaskStatus? Status { get; set; }

    public TaskCategory? Category { get; set; }

    public int? Priority { get; set; }
}
