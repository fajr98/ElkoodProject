namespace ElkoodProject.Domain.Tasks.Models;

using System;

public class TaskFilter
{
    public string? Name { get; set; }

    public int? DiedLineInHours { get; set; }

    public TaskStatus? Status { get; set; }

    public TaskCategory? Category { get; set; }

    public int? Priority { get; set; }
}
