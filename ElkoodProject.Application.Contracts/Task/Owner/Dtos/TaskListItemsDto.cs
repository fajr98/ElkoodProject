namespace ElkoodProject.Application.Contracts.Task.Owner.Dtos;

using System.Collections.Generic;

public class TaskListItemsDto
{
    public IEnumerable<TaskDto>? Items { get; set; }
}
